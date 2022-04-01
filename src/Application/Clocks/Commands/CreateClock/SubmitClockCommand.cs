using System.Net.Mime;
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

namespace RendezVous.Application.Clocks.Commands.CreateClock;

public class SubmitClockCommand : IRequest
{
    public Guid AssignmentId { get; set; }
    public ClockType ClockType { get; set; }
    public Coordinates Coordinates { get; set; } = null!;
}

public class SubmitClockCommandValidator : AbstractValidator<SubmitClockCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRendezVousDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly BusinessOptions _businessOptions;

    public SubmitClockCommandValidator(
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

    public override async Task<ValidationResult> ValidateAsync(
        ValidationContext<SubmitClockCommand> context,
        CancellationToken ct = new())
    {
        var result = await base.ValidateAsync(context, ct);

        if (!result.IsValid) { return result; }

        var request = context.InstanceToValidate;

        var employee = await _dbContext.Employees
            .SingleAsync(x => x.ProviderId == _currentUserService.ProviderId, ct);

        var assignment = await GetAssignment(request, ct);

        if (assignment is null)
        {
            context.AddMissingEntityFailure(nameof(Assignment), request.AssignmentId);
            return result;
        }

        context.AddFailureIf(
            assignment.EmployeeId != employee.Id,
            "You cannot clock in/out for another employee.");

        EnsureTimings(assignment, context);
        EnsureDistanceFromJob(assignment, context);

        return result;
    }

    private void EnsureTimings(
        Assignment assignment,
        ValidationContext<SubmitClockCommand> context)
    {
        var isClockedIn = assignment.Clocks.Any(x => x.Type == ClockType.In);
        var isClockedOut = assignment.Clocks.Any(x => x.Type == ClockType.Out);

        if (context.InstanceToValidate.ClockType == ClockType.In)
        {
            context.AddFailureIf(
                isClockedIn,
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
                !isClockedIn,
                "You must clock in before clocking out.");

            context.AddFailureIf(
                isClockedOut,
                "You have already clocked out of this job");

            context.AddFailureIf(
                _dateTime.Now > assignment.Job.End.Add(_businessOptions.LateClockOutThreshold),
                "It is too late to clock out of this job.");
        }
    }

    private void EnsureDistanceFromJob(
        Assignment assignment,
        ValidationContext<SubmitClockCommand> context)
    {
        var jobLocation = assignment.Job.Location;
        var distanceFromJob = jobLocation.Coordinates.Distance(context.InstanceToValidate.Coordinates);

        context.AddFailureIf(
            distanceFromJob.Meters > jobLocation.Radius.Meters,
            $"Your current location does not match the location for this job ({jobLocation.Title})");
    }

    private Task<Assignment?> GetAssignment(SubmitClockCommand request, CancellationToken ct)
    {
        return _dbContext.Assignments
            .Include(x => x.Job)
            .ThenInclude(x => x.Location)
            .Include(x => x.Clocks)
            .SingleOrDefaultAsync(x => x.Id == request.AssignmentId, ct);
    }
}

public class SubmitClockCommandHandler : IRequestHandler<SubmitClockCommand>
{
    private readonly IRendezVousDbContext _rendezVousDbContext;
    private readonly IDateTime _dateTime;

    public SubmitClockCommandHandler(
        IRendezVousDbContext rendezVousDbContext,
        IDateTime dateTime)
    {
        _rendezVousDbContext = rendezVousDbContext;
        _dateTime = dateTime;
    }

    public async Task<Unit> Handle(SubmitClockCommand request, CancellationToken ct)
    {
        var clock = new Clock
        {
            Id = Guid.NewGuid(),
            At = _dateTime.Now,
            Type = request.ClockType,
            Coordinates = request.Coordinates,
            AssignmentId = request.AssignmentId
        };

        await _rendezVousDbContext.Clocks.AddAsync(clock, ct);
        await _rendezVousDbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
