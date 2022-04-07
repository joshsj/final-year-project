using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace RendezVous.WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class RendezVousControllerBase : ControllerBase
{
    protected ISender Mediator { get; }

    public RendezVousControllerBase(ISender sender)
    {
        Mediator = sender;
    }
}
