using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RendezVous.Application.Jobs.Queries.GetAssignments;
using RendezVous.Application.Jobs.Queries.GetJobs;

namespace RendezVous.WebUI.Controllers;

[Authorize]
public class JobController : RendezVousControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BriefJobDto>>> Get()
    {
        return Ok(await Mediator.Send(new GetJobsQuery()));
    }

    [HttpGet("assignment")]
    public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignments(
        [FromQuery] GetAssignmentsQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}
