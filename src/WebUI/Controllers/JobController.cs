using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}
