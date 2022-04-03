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

public class SubmitUnconfirmedClockCommand : BaseSubmitClockCommand
{
}

public class SubmitUnconfirmedClockCommandValidator
    : BaseSubmitClockCommandValidator<SubmitUnconfirmedClockCommand>
{
    public SubmitUnconfirmedClockCommandValidator(
        ICurrentUserService currentUserService,
        IRendezVousDbContext dbContext,
        IDateTime dateTime,
        IOptions<BusinessOptions> businessOptions)
        : base(currentUserService, dbContext, dateTime, businessOptions)
    {
    }

    protected override Task EnsureConfirmation(
        Assignment assignment,
        Employee employee,
        ValidationContext<SubmitUnconfirmedClockCommand> context,
        CancellationToken ct)
    {
        context.AddFailureIf(
            assignment.Clocks.Any(),
            "Clocking in/out of this job requires confirmation with another employee.");
        
        return Task.CompletedTask;
    }
}

public class SubmitUnconfirmedClockCommandHandler : IRequestHandler<SubmitUnconfirmedClockCommand>
{
    private readonly IRendezVousDbContext _dbContext;
    private readonly IDateTime _dateTime;

    public SubmitUnconfirmedClockCommandHandler(
        IRendezVousDbContext dbContext,
        IDateTime dateTime)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
    }

    public async Task<Unit> Handle(SubmitUnconfirmedClockCommand request, CancellationToken ct)
    {
        var clock = new Clock
        {
            Id = Guid.NewGuid(),
            At = _dateTime.Now,
            Type = request.ClockType,
            Coordinates = request.Coordinates,
            AssignmentId = request.AssignmentId,
            ParentId = null
        };

        await _dbContext.Clocks.AddAsync(clock, ct);
        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
