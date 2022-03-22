using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RendezVouz.Domain.Exceptions;

namespace RendezVouz.Controllers.Common.Filters;

public class RendezVousExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, int> _statusCodes = new Dictionary<Type, int>
        {
             {typeof(NotFoundException), StatusCodes.Status400BadRequest},
             {typeof(AuthenticationException), StatusCodes.Status401Unauthorized},
             {typeof(AuthorizationException), StatusCodes.Status403Forbidden},
        };
    public override void OnException(ExceptionContext context)
    {
        var type = context.Exception.GetType();

        context.Result = new ObjectResult(null)
        {
            StatusCode = _statusCodes.ContainsKey(type)
                ? _statusCodes[type]
                : StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;

        base.OnException(context);
    }
}