using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RendezVous.Application.Clocks.Commands.CreateClockConfirmationCode;
using RendezVous.Application.Clocks.Commands.SubmitClock;

namespace RendezVous.WebUI.Controllers;

public class ClockController : RendezVousControllerBase
{
    public ClockController(ISender sender) : base(sender)
    {
    }
    
    [HttpPost("submission/unconfirmed")]
    [SwaggerResponse(typeof(void))]
    public async Task<ActionResult> SubmitUnconfirmed(SubmitUnconfirmedClockCommand request)
    {
        await Mediator.Send(request);

        return NoContent();
    }
    
    [HttpPost("submission/confirmed")]
    [SwaggerResponse(typeof(void))]
    public async Task<ActionResult> SubmitConfirmed(SubmitConfirmedClockCommand request)
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
