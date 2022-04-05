// CreateClockConfirmationCodeCommandValidator.cs

// Data integrity
RuleFor(x => x.ConfirmeeAssignmentId).NotNull();

// Referential integrity
if (confirmeeAssignment is null)
{
    context.AddMissingEntityFailure(
        nameof(Assignment),
        request.ConfirmeeAssignmentId);
}

// Authorization + Business rules
EnsureCanConfirmClock(
    context, confirmer, confirmeeAssignment, ct);

EnsureClockCanBeConfirmed(
    context, confirmeeAssignment);

