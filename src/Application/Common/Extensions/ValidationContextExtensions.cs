using FluentValidation;

namespace RendezVous.Application.Common.Extensions;

public static class ValidationContextExtensions
{
    public static ValidationContext<T> AddFailureIf<T>(
        this ValidationContext<T> context,
        bool condition,
        string errorMessage)
    {
        if (condition)
        {
            context.AddFailure(errorMessage);
        }

        return context;
    }

    public static ValidationContext<T> AddMissingEntityFailure<T>(
        this ValidationContext<T> context,
        string entityName,
        Guid? entityId = null)
    {
        var idMessage = entityId is null
            ? ""
            : $" with ID {entityId}";

        context.AddFailure($"{entityName}{idMessage} does not exist'");

        return context;
    }
}
