using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RendezVous.Application.Common.Extensions;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Options;

namespace RendezVous.Application.Clocks.Commands.SubmitClock;

public class SubmitConfirmedClockCommand : BaseSubmitClockCommand
{
    public string ConfirmationTokenValue { get; set; } = null!;
}

public class SubmitConfirmedClockCommandValidator
    : BaseSubmitClockCommandValidator<SubmitConfirmedClockCommand>
{
    public SubmitConfirmedClockCommandValidator(
        ICurrentUserService currentUserService,
        IRendezVousDbContext dbContext,
        IDateTime dateTime,
        IOptions<BusinessOptions> businessOptions)
        : base(currentUserService, dbContext, dateTime, businessOptions)
    {
        RuleFor(x => x.ConfirmationTokenValue)
            .NotNull()
            .NotEmpty();
    }

    protected override async Task EnsureConfirmation(
        Assignment assignment,
        Employee employee,
        ValidationContext<SubmitConfirmedClockCommand> context,
        CancellationToken ct)
    {
        var confirmationToken = await _dbContext.ConfirmationTokens
            .SingleOrDefaultAsync(x => x.Value == context.InstanceToValidate.ConfirmationTokenValue, ct);

        if (confirmationToken is null)
        {
            context.AddFailure("The confirmation token is invalid.");
            return;
        }

        context.AddFailureIf(
            confirmationToken.ConfirmeeAssignmentId != assignment.Id, 
            "This confirmation token was created for another employee.");

        context.AddFailureIf(
            _dateTime.Now < confirmationToken.ExpiresAt,
            "The confirmation token has expired.");
    }
}

public class SubmitConfirmedClockCommandHandler : IRequestHandler<SubmitConfirmedClockCommand>
{
    private readonly IRendezVousDbContext _dbContext;
    private readonly IDateTime _dateTime;

    public SubmitConfirmedClockCommandHandler(
        IRendezVousDbContext dbContext,
        IDateTime dateTime)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
    }

    public async Task<Unit> Handle(SubmitConfirmedClockCommand request, CancellationToken ct)
    {
        var confirmationToken = await _dbContext.ConfirmationTokens
            .Include(x => x.ConfirmerAssignment)
            .SingleAsync(x => x.Value == request.ConfirmationTokenValue, ct);

        var parent = confirmationToken.ConfirmerAssignment.ClockIn;
        
        var clock = new Clock
        {
            Id = Guid.NewGuid(),
            At = _dateTime.Now,
            Type = request.ClockType,
            Coordinates = request.Coordinates,
            AssignmentId = request.AssignmentId,
            ParentId = parent!.Id
        };

        await _dbContext.Clocks.AddAsync(clock, ct);
        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
