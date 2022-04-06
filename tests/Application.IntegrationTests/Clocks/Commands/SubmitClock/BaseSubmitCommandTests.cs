using RendezVous.Application.Clocks.Commands.SubmitClock;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Clocks.Commands.SubmitClock;

public class BaseSubmitClockCommandTests : UserTestBase
{
    [Test]
    public void FailsWhenAssignmentIdIsInvalid()
    {
        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = Guid.Empty, ClockType = ClockType.In, Coordinates = new Coordinates(1, 1)
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public void FailsWhenCoordinatesIsInvalid()
    {
        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = Guid.NewGuid(), ClockType = ClockType.In, Coordinates = null!
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public void FailsWhenAssignmentDoesNotExist()
    {
        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = Guid.NewGuid(), ClockType = ClockType.In, Coordinates = new Coordinates(1, 1),
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenClockingForAnotherEmployee()
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
        var otherAssignment = new Assignment
        {
            Id = Guid.NewGuid(), JobId = job.Id, EmployeeId = otherEmployee.Id, Notes = ""
        };
        await Add(otherEmployee);
        await Add(job);
        await Add(otherAssignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = otherAssignment.Id, // not mine
            ClockType = ClockType.In,
            Coordinates = new Coordinates(1, 1),
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenTooFarFromJob()
    {
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
                Coordinates = new Coordinates(1, 1)
            }
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(), Notes = "", JobId = job.Id, EmployeeId = CurrentUser.Id
        };
        await Add(job);
        await Add(assignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = assignment.Id,
            ClockType = ClockType.In,
            Coordinates = new Coordinates(-1, -1) // very far away
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }
}
