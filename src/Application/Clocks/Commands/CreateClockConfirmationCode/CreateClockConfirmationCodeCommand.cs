using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RendezVous.Application.Common.Extensions;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Options;

namespace RendezVous.Application.Clocks.Commands.CreateClockConfirmationCode;

public class CreateClockConfirmationCodeCommand : IRequest<ConfirmationCodeDto>
{
    public Guid ConfirmeeAssignmentId { get; set; }
}

public class CreateClockConfirmationCodeCommandValidator
    : AbstractValidator<CreateClockConfirmationCodeCommand>
{
    private readonly IRendezVousDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateClockConfirmationCodeCommandValidator(
        IRendezVousDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;

        RuleFor(x => x.ConfirmeeAssignmentId)
            .NotNull()
            .NotEmpty();
    }

    public override async Task<ValidationResult> ValidateAsync(
        ValidationContext<CreateClockConfirmationCodeCommand> context,
        CancellationToken ct = new())
    {
        var result = await base.ValidateAsync(context, ct);
        if (!result.IsValid) { return result; }

        var request = context.InstanceToValidate;

        var confirmer = await _dbContext.Employees.SingleAsync(
            x => x.ProviderId == _currentUserService.ProviderId, ct);

        var confirmeeAssignment = await _dbContext.Assignments
            .Include(x => x.Clocks)
            .SingleOrDefaultAsync(x => x.Id == request.ConfirmeeAssignmentId, ct);

        if (confirmeeAssignment is null)
        {
            context.AddMissingEntityFailure(nameof(Assignment), request.ConfirmeeAssignmentId);
            return result;
        }

        await EnsureCanConfirmClock(context, confirmer, confirmeeAssignment, ct);
        EnsureClockCanBeConfirmed(context, confirmeeAssignment);

        return result;
    }

    private async Task EnsureCanConfirmClock(
        ValidationContext<CreateClockConfirmationCodeCommand> context,
        Employee confirmer,
        Assignment confirmeeAssignment,
        CancellationToken ct)
    {
        context.AddFailureIf(
            confirmeeAssignment.EmployeeId == confirmer.Id,
            "You cannot confirm your own clock in/out");

        var confirmerAssignment = await _dbContext.Assignments
            .Include(x => x.Clocks)
            .SingleOrDefaultAsync(x =>
                    x.EmployeeId == confirmer.Id &&
                    x.JobId == confirmeeAssignment.JobId,
                ct);

        if (confirmerAssignment is null)
        {
            context.AddFailure("You cannot confirm a clock on a job to which you are not assigned.");
            return;
        }

        context.AddFailureIf(
            confirmerAssignment.ClockIn is null,
            "You must be clocked in to confirm another clock.");
    }

    private void EnsureClockCanBeConfirmed(
        ValidationContext<CreateClockConfirmationCodeCommand> context,
        Assignment confirmeeAssignment)
    {
        context.AddFailureIf(
            confirmeeAssignment.ClockOut is not null,
            "This employee has clocked out; they do not need a clock confirmed.");
    }
}

public class CreateClockConfirmationCodeCommandHandler : IRequestHandler<CreateClockConfirmationCodeCommand, ConfirmationCodeDto>
{
    private readonly IRendezVousDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IBarcodeService _barcodeService;
    private readonly BusinessOptions _businessOptions;

    public CreateClockConfirmationCodeCommandHandler(
        IRendezVousDbContext dbContext,
        ICurrentUserService currentUserService,
        IDateTime dateTime,
        IBarcodeService barcodeService,
        IOptions<BusinessOptions> businessOptions)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
        _barcodeService = barcodeService;
        _businessOptions = businessOptions.Value;
    }

    public async Task<ConfirmationCodeDto> Handle(CreateClockConfirmationCodeCommand request, CancellationToken ct)
    {
        var confirmer = await _dbContext.Employees.SingleAsync(
            x => x.ProviderId == _currentUserService.ProviderId, ct);
        
        var confirmeeAssignment = await _dbContext.Assignments
            .SingleAsync(x => x.Id == request.ConfirmeeAssignmentId, ct);

        var confirmerAssignment = await _dbContext.Assignments
            .SingleAsync(x => x.JobId == confirmeeAssignment.JobId && x.EmployeeId == confirmer.Id , ct);
        
        var confirmationToken = new ConfirmationToken
        {
            Id = Guid.NewGuid(),
            Value = Guid.NewGuid().ToString(),
            ExpiresAt = _dateTime.Now.Add(_businessOptions.ConfirmationTokenWindow),
            ConfirmerAssignmentId = confirmerAssignment.Id,
            ConfirmeeAssignmentId = request.ConfirmeeAssignmentId
        };

        await _dbContext.ConfirmationTokens.AddAsync(confirmationToken, ct);
        await _dbContext.SaveChangesAsync(ct);

        return new ConfirmationCodeDto
        {
            SvgSource = _barcodeService.CreateSvg(confirmationToken.Value),
            TimeRemaining = (int) _businessOptions.ConfirmationTokenWindow.TotalSeconds
        };
    }
}
