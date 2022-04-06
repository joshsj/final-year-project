using RendezVous.Application.Clocks.Commands.SubmitClock;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Clocks.Commands.SubmitClock;

public class BaseSubmitCommand_ClockInTests : UserTestBase
{
    [Test]
    public async Task FailsWhenAlreadyClockedIn()
    {
        var coordinates = new Coordinates(1, 1);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Start = DateTime.UtcNow.AddHours(-1),
            End = DateTime.UtcNow.AddHours(1),
            Location = new Location
            {
                Id = Guid.NewGuid(),
                Title = "",
                Radius = new(1),
                Coordinates = coordinates
            }
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            EmployeeId = CurrentUser.Id,
            Clocks = {new Clock
            {
                Id =Guid.NewGuid(),
                // clocked in
                Type = ClockType.In,
                Coordinates = coordinates
            }}
        };
        await Add(job);
        await Add(assignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = assignment.Id, ClockType = ClockType.In, Coordinates = coordinates
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenJobHasEnded()
    {
        var coordinates = new Coordinates(1, 1);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Start = DateTime.UtcNow.AddDays(-2),
            // already ended
            End = DateTime.UtcNow.AddDays(-1),
            Location = new Location
            {
                Id = Guid.NewGuid(),
                Title = "",
                Radius = new(1),
                Coordinates = coordinates
            }
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            EmployeeId = CurrentUser.Id
        };
        await Add(job);
        await Add(assignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = assignment.Id, ClockType = ClockType.In, Coordinates = coordinates
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }


    [Test]
    public async Task FailsWhenTooEarly()
    {
        var coordinates = new Coordinates(1, 1);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            // not yet started
            Start = DateTime.UtcNow.AddHours(1),
            End = DateTime.UtcNow.AddHours(3),
            Location = new Location
            {
                Id = Guid.NewGuid(),
                Title = "",
                Radius = new(1),
                Coordinates = coordinates
            }
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            EmployeeId = CurrentUser.Id
        };
        await Add(job);
        await Add(assignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = assignment.Id, ClockType = ClockType.In, Coordinates = coordinates
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }
}
