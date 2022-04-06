using RendezVous.Application.Clocks.Commands.SubmitClock;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Clocks.Commands.SubmitClock;

public class SubmitUnconfirmedClockCommandTests : UserTestBase
{
    [Test]
    public async Task PassesForValidRequest()
    {
        var coordinates = new Coordinates(1, 1);

        var otherEmployee = new Employee
        {
            Id = Guid.NewGuid(), ProviderId = CreateProviderId(), Name = "other", Email = "other@email.com"
        };

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            // correct timing
            Start = DateTime.UtcNow.AddHours(-1),
            End = DateTime.UtcNow.AddHours(1),
            Location = new Location
            {
                Id = Guid.NewGuid(),
                Title = "",
                Radius = new(1),
                // correct location
                Coordinates = coordinates
            }
        };

        var myAssignment = new Assignment
        {
            Id = Guid.NewGuid(), Notes = "", JobId = job.Id, EmployeeId = CurrentUser.Id
        };
        var otherAssignment = new Assignment
        {
            Id = Guid.NewGuid(), Notes = "", JobId = job.Id, EmployeeId = otherEmployee.Id
        };
        await Add(otherEmployee);
        await Add(job);
        await Add(myAssignment, otherAssignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = myAssignment.Id, ClockType = ClockType.In, Coordinates = coordinates
        };

        var result = await Send(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(await Count<Clock>(), Is.EqualTo(1));
    }

    [Test]
    public async Task FailsWhenAnotherEmployeeHasClockedIn()
    {
        var coordinates = new Coordinates(1, 1);

        var otherEmployee = new Employee
        {
            Id = Guid.NewGuid(), ProviderId = CreateProviderId(), Name = "other", Email = "other@email.com"
        };

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            // correct timing
            Start = DateTime.UtcNow.AddHours(-1),
            End = DateTime.UtcNow.AddHours(1),
            Location = new Location
            {
                Id = Guid.NewGuid(),
                Title = "",
                Radius = new(1),
                // correct location
                Coordinates = coordinates
            }
        };

        var myAssignment = new Assignment
        {
            Id = Guid.NewGuid(), Notes = "", JobId = job.Id, EmployeeId = CurrentUser.Id
        };
        var otherAssignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            EmployeeId = otherEmployee.Id,
            // other employee clocked in 
            Clocks = {new() {Id = Guid.NewGuid(), Type = ClockType.In, Coordinates = coordinates,}}
        };
        await Add(otherEmployee);
        await Add(job);
        await Add(myAssignment, otherAssignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = myAssignment.Id, ClockType = ClockType.In, Coordinates = coordinates
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }
}
