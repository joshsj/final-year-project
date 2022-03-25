using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace RendezVous.WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class RendezVousControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
