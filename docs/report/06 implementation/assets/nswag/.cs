// JobController.cs

[HttpGet]
public async Task<IEnumerable<BriefJobDto>> Get()
{
    return Ok(await Mediator.Send(new GetJobsQuery()));
}