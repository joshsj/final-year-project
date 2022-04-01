using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RendezVous.Application.Clocks.Commands.CreateClock;

namespace RendezVous.WebUI.Controllers;

[Authorize]
public class ClockController : RendezVousControllerBase
{
    [HttpPost("submission")]
    public async Task<ActionResult> Submit(SubmitClockCommand request)
    {
        await Mediator.Send(request);

        return Ok();
    }
}
