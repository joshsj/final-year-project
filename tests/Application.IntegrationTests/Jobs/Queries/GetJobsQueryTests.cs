using RendezVous.Application.Jobs.Queries.GetJobs;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Jobs.Queries;


public class GetJobsQueryTests : UserTestBase
{
    [Test]
    public async Task PassesForValidRequest()
    {
        var job1 = new Job
        {
            Id = Guid.NewGuid(),
            Title = "job",
            Description = "",
            Location = new Location
            {
                Id = Guid.NewGuid(), Title = ""
            },
        };
        var job2 = new Job
        {
            Id = Guid.NewGuid(),
            Title = "job",
            Description = "",
            Location = new Location
            {
                Id = Guid.NewGuid(), Title = ""
            },
        };

        await Add(job1, job2);

        var result = await Send(new GetJobsQuery());

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
    }
}
