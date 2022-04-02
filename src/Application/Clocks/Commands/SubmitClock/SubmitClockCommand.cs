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

public class SubmitClockCommand : IRequest
{
    public string ConfirmationTokenValue { get; set; } = null!;
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

        RuleFor(x => x.ConfirmationTokenValue)
            .NotNull()
            .NotEmpty();

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

        var confirmationToken = await GetConfirmationToken(request, ct);

        if (confirmationToken is null)
        {
            context.AddFailure("This confirmation code is invalid.");
            return result;
        }
        
        EnsureValidToken(confirmationToken, context);

        context.AddFailureIf(
            confirmationToken.Assignment.EmployeeId != employee.Id,
            "You cannot clock in/out for another employee.");

        EnsureTimings(confirmationToken, context);
        EnsureDistanceFromJob(confirmationToken, context);

        return result;
    }

    private void EnsureValidToken(
        ConfirmationToken confirmationToken,
        ValidationContext<SubmitClockCommand> context)
    {
        context.AddFailureIf(
            _dateTime.Now > confirmationToken.ExpiresAt,
            "This confirmation code has expired.");
    }

    private void EnsureTimings(
        ConfirmationToken confirmationToken,
        ValidationContext<SubmitClockCommand> context)
    {
        var assignment = confirmationToken.Assignment;
        
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

    private void EnsureDistanceFromJob(
        ConfirmationToken confirmationToken,
        ValidationContext<SubmitClockCommand> context)
    {
        var jobLocation = confirmationToken.Assignment.Job.Location;
        var distanceFromJob = jobLocation.Coordinates.Distance(context.InstanceToValidate.Coordinates);

        context.AddFailureIf(
            distanceFromJob.Meters > jobLocation.Radius.Meters,
            $"Your current location does not match the location for this job ({jobLocation.Title})");
    }

    private Task<ConfirmationToken?> GetConfirmationToken(SubmitClockCommand request, CancellationToken ct)
    {
        return _dbContext.ConfirmationTokens
            .Include(x => x.Assignment)
            .ThenInclude(x => x.Job)
            .ThenInclude(x => x.Location)
            .Include(x => x.Assignment)
            .ThenInclude(x => x.Clocks)
            .SingleOrDefaultAsync(x => x.Value == request.ConfirmationTokenValue, ct);
    }
}

public class SubmitClockCommandHandler : IRequestHandler<SubmitClockCommand>
{
    private readonly IRendezVousDbContext _dbContext;
    private readonly IDateTime _dateTime;

    public SubmitClockCommandHandler(
        IRendezVousDbContext dbContext,
        IDateTime dateTime)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
    }

    public async Task<Unit> Handle(SubmitClockCommand request, CancellationToken ct)
    {
        var confirmationToken = await _dbContext.ConfirmationTokens.SingleAsync(
            x => x.Value == request.ConfirmationTokenValue, 
            ct);
        
        var clock = new Clock
        {
            Id = Guid.NewGuid(),
            At = _dateTime.Now,
            Type = request.ClockType,
            Coordinates = request.Coordinates,
            AssignmentId = confirmationToken.AssignmentId
        };

        await _dbContext.Clocks.AddAsync(clock, ct);
        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
