using RendezVous.Application.Jobs.Queries.GetAssignments;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Jobs.Queries;

public class GetAssignmentsQueryTests : UserTestBase
{
    [Test]
    public async Task PassesWhenRequestIsValid()
    {
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "notes",
            EmployeeId = CurrentUser.Id,
            Job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "job", Description = "", Location = new Location {Id = Guid.NewGuid(), Title = ""},
            }
        };
        await Add(assignment);

        var request = new GetAssignmentsQuery {JobId = assignment.Job.Id};

        var result = await Send(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public void FailsWhenJobDoesNotExist()
    {
        var request = new GetAssignmentsQuery {JobId = Guid.NewGuid()};

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public void FailsWhenJobIdIsInvalid()
    {
        var request = new GetAssignmentsQuery {JobId = Guid.Empty};

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }
}
