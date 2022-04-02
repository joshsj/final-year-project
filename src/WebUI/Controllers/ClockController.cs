using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RendezVous.Application.Clocks.Commands;
using RendezVous.Application.Clocks.Commands.CreateClockConfirmationCode;
using RendezVous.Application.Clocks.Commands.SubmitClock;

namespace RendezVous.WebUI.Controllers;

[Authorize]
public class ClockController : RendezVousControllerBase
{
    [HttpPost("submission")]
    [SwaggerResponse(typeof(void))]
    public async Task<ActionResult> Submit(SubmitClockCommand request)
    {
        await Mediator.Send(request);

        return NoContent();
    }

    [HttpGet("confirmation-code")]
    public async Task<ActionResult<ConfirmationCodeDto>> GetConfirmationCode(
        [FromQuery] CreateClockConfirmationCodeCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}
