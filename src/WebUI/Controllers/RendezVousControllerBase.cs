using MediatR;

using Microsoft.AspNetCore.Mvc;
using RendezVous.Application.Common.Security;

namespace RendezVous.WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class RendezVousControllerBase : ControllerBase
{
    protected ISender Mediator { get; }

    public RendezVousControllerBase(ISender sender)
    {
        Mediator = sender;
    }
}
