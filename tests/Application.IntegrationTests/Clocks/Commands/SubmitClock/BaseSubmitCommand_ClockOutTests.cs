using RendezVous.Application.Clocks.Commands.SubmitClock;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Clocks.Commands.SubmitClock;

public class BaseSubmitCommand_ClockOutTests : UserTestBase
{
    [Test]
    public async Task FailsWhenNotClockedIn()
    {
        var coordinates = new Coordinates(1, 1);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Start = DateTime.UtcNow.AddHours(-1),
            End = DateTime.UtcNow.AddHours(1),
            Location = new Location {Id = Guid.NewGuid(), Title = "", Radius = new(1), Coordinates = coordinates}
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(), Notes = "", JobId = job.Id, EmployeeId = CurrentUser.Id,
            // not clocked in
        };
        await Add(job);
        await Add(assignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = assignment.Id, ClockType = ClockType.Out, Coordinates = coordinates
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenAlreadyClockedOut()
    {
        var coordinates = new Coordinates(1, 1);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Start = DateTime.UtcNow.AddHours(-1),
            End = DateTime.UtcNow.AddHours(1),
            Location = new Location {Id = Guid.NewGuid(), Title = "", Radius = new(1), Coordinates = coordinates}
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            EmployeeId = CurrentUser.Id,
            Clocks =
            {
                new() {Id = Guid.NewGuid(), Type = ClockType.In, Coordinates = coordinates},
                // clocked out
                new() {Id = Guid.NewGuid(), Type = ClockType.Out, Coordinates = coordinates}
            }
        };
        await Add(job);
        await Add(assignment);

        var request = new SubmitUnconfirmedClockCommand
        {
            AssignmentId = assignment.Id, ClockType = ClockType.Out, Coordinates = coordinates
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenTooLate()
    {
        var coordinates = new Coordinates(1, 1);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Start = DateTime.UtcNow.AddHours(-10),
            // finished a long time ago
            End = DateTime.UtcNow.AddHours(-7),
            Location = new Location {Id = Guid.NewGuid(), Title = "", Radius = new(1), Coordinates = coordinates}
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            EmployeeId = CurrentUser.Id,
            Clocks = {new() {Id = Guid.NewGuid(), Type = ClockType.In, Coordinates = coordinates},}
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
