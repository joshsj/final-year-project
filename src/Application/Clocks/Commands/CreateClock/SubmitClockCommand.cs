using System.Data;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RendezVous.Application.Common.Extensions;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Enums;
using RendezVous.Domain.Models;

namespace RendezVous.Application.Clocks.Commands.CreateClock;

public class SubmitClockCommand : IRequest
{
    public Guid ClockId { get; set; }
    public Coordinates Coordinates { get; set; } = null!;
}

public class SubmitClockCommandValidator : AbstractValidator<SubmitClockCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRendezVousDbContext _dbContext;

    public SubmitClockCommandValidator(
        ICurrentUserService currentUserService,
        IRendezVousDbContext dbContext)
    {
        _currentUserService = currentUserService;
        _dbContext = dbContext;

        RuleFor(x => x.ClockId)
            .NotNull();
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<SubmitClockCommand> context,
        CancellationToken ct = new())
    {
        var result = await base.ValidateAsync(context, ct);

        if (!result.IsValid) { return result; }

        var request = context.InstanceToValidate;

        var employee = await _dbContext.Employees
            .SingleAsync(x => x.ProviderId == _currentUserService.ProviderId, ct);

        var clock = await _dbContext.Clocks
            .Include(x => x.Assignment)
            .ThenInclude(x => x.Job)
            .ThenInclude(x => x.Location)
            .SingleOrDefaultAsync(x => x.Id == request.ClockId, ct);

        if (clock is null)
        {
            context.AddMissingEntityFailure(nameof(Clock), request.ClockId);
            return result;
        }

        context.AddFailureIf(
            clock.Assignment.EmployeeId != employee.Id,
            "You cannot clock in/out for another employee.");

        context.AddFailureIf(
            clock.ActualAt.HasValue,
            "This clock has already been submitted.");

        EnsureDistanceFromJob(clock, context);
        await EnsureInBeforeOut(clock, context, ct);

        return result;
    }

    private async Task EnsureInBeforeOut(
        Clock clock,
        ValidationContext<SubmitClockCommand> context,
        CancellationToken ct)
    {
        if (clock.Type == ClockType.In) { return; }

        if (await _dbContext.Clocks.AnyAsync(x =>
                x.AssignmentId == clock.AssignmentId &&
                x.Type == ClockType.In, ct))
        {
            return;
        }
        
        context.AddFailure("You must clock in before clocking out");
    }

    private void EnsureDistanceFromJob(
        Clock clock,
        ValidationContext<SubmitClockCommand> context)
    {
        var jobLocation = clock.Assignment.Job.Location;
        var distanceFromJob = jobLocation.Coordinates.Distance(context.InstanceToValidate.Coordinates);

        context.AddFailureIf(
            distanceFromJob.Meters > jobLocation.Radius.Meters,
            $"Your current location does not match the location for this job ({jobLocation.Title})");
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
        var clock = await _rendezVousDbContext.Clocks.FirstAsync(x => x.Id == request.ClockId, ct);

        clock.ActualAt = _dateTime.Now;

        await _rendezVousDbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
