using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RendezVous.Application.Common.Extensions;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Enums;
using RendezVous.Domain.Models;
using RendezVous.Domain.Options;

namespace RendezVous.Application.Clocks.Commands.SubmitClock;

public class BaseSubmitClockCommand : IRequest
{
    public Guid AssignmentId { get; set; }
    public ClockType ClockType { get; set; }
    public Coordinates Coordinates { get; set; } = null!;
}

public abstract class BaseSubmitClockCommandValidator<T> 
    : AbstractValidator<T> where T : BaseSubmitClockCommand
{
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IRendezVousDbContext _dbContext;
    protected readonly IDateTime _dateTime;
    protected readonly BusinessOptions _businessOptions;

    public BaseSubmitClockCommandValidator(
        ICurrentUserService currentUserService,
        IRendezVousDbContext dbContext,
        IDateTime dateTime,
        IOptions<BusinessOptions> businessOptions)
    {
        _currentUserService = currentUserService;
        _dbContext = dbContext;
        _dateTime = dateTime;
        _businessOptions = businessOptions.Value;

        RuleFor(x => x.AssignmentId)
            .NotNull();
        
        RuleFor(x => x.Coordinates)
            .NotNull();
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken ct = new())
    {
        var result = await base.ValidateAsync(context, ct);
        if (!result.IsValid) { return result; }

        var request = context.InstanceToValidate;

        var employee = await _dbContext.Employees.SingleAsync(x => x.ProviderId == _currentUserService.ProviderId, ct);

        var assignment = await GetAssignment(context, ct);

        if (assignment is null)
        {
            context.AddMissingEntityFailure(nameof(Assignment), request.AssignmentId);
            return result;
        }

        context.AddFailureIf(
            assignment.EmployeeId != employee.Id,
            "You cannot clock in/out for another employee.");

        await EnsureConfirmation(assignment, employee, context, ct);
        EnsureTimings(assignment, context);
        EnsureDistanceFromJob(assignment, context);

        return result;
    }

    /// <summary>Checks confirmation token or principle confirmation</summary>
    protected abstract Task EnsureConfirmation(
        Assignment assignment,
        Employee employee,
        ValidationContext<T> context,
        CancellationToken ct);

    private void EnsureTimings(Assignment assignment, ValidationContext<T> context)
    {
        if (context.InstanceToValidate.ClockType == ClockType.In)
        {
            context.AddFailureIf(
                assignment.ClockIn is not null,
                "You have already clocked in for this job.");

            context.AddFailureIf(
                _dateTime.Now > assignment.Job.End,
                "You cannot clock in for a job which has ended.");

            context.AddFailureIf(
                _dateTime.Now < assignment.Job.Start.Subtract(_businessOptions.EarlyClockInThreshold),
                "It is too early to clock in for this job.");
        }
        else
        {
            context.AddFailureIf(
                assignment.ClockIn is null,
                "You must clock in before clocking out.");

            context.AddFailureIf(
                assignment.ClockOut is not null,
                "You have already clocked out of this job");

            context.AddFailureIf(
                _dateTime.Now > assignment.Job.End.Add(_businessOptions.LateClockOutThreshold),
                "It is too late to clock out of this job.");
        }
    }

    private void EnsureDistanceFromJob(Assignment assignment, ValidationContext<T> context)
    {
        var jobLocation = assignment.Job.Location;
        var distanceFromJob = jobLocation.Coordinates.Distance(context.InstanceToValidate.Coordinates);

        context.AddFailureIf(
            distanceFromJob.Meters > jobLocation.Radius.Meters,
            $"Your current location does not match the location for this job ({jobLocation.Title})");
    }

    private async Task<Assignment?> GetAssignment( ValidationContext<T> context, CancellationToken ct = new())
    {
        return await _dbContext.Assignments
            .Include(x => x.Job)
            .ThenInclude(x => x.Location)
            .Include(x => x.Clocks)
            .SingleOrDefaultAsync(x => x.Id == context.InstanceToValidate.AssignmentId, ct);
    }
}
