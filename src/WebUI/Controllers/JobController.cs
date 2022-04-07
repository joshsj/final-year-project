using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RendezVous.Application.Jobs.Queries.GetAssignments;
using RendezVous.Application.Jobs.Queries.GetJobs;

namespace RendezVous.WebUI.Controllers;

public class JobController : RendezVousControllerBase
{
    public JobController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IList<BriefJobDto>>> Get()
    {
        return Ok(await Mediator.Send(new GetJobsQuery()));
    }

    [HttpGet("assignment")]
    public async Task<ActionResult<IList<AssignmentDto>>> GetAssignments(
        [FromQuery] GetAssignmentsQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}
