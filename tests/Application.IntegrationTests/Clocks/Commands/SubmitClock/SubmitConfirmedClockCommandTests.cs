using RendezVous.Application.Clocks.Commands.SubmitClock;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Clocks.Commands.SubmitClock;

public class SubmitConfirmedClockCommandTests : UserTestBase
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
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            EmployeeId = otherEmployee.Id,
            Clocks = {new Clock {Id = Guid.NewGuid(), Type = ClockType.In, Coordinates = coordinates}}
        };

        var confirmationToken = new ConfirmationToken
        {
            Id = Guid.NewGuid(),
            Value = "confirmation",
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            ConfirmeeAssignmentId = myAssignment.Id,
            ConfirmerAssignmentId = otherAssignment.Id
        };

        await Add(otherEmployee);
        await Add(job);
        await Add(myAssignment, otherAssignment);
        await Add(confirmationToken);

        var request = new SubmitConfirmedClockCommand
        {
            AssignmentId = myAssignment.Id,
            ClockType = ClockType.In,
            Coordinates = coordinates,
            ConfirmationTokenValue = confirmationToken.Value
        };

        var result = await Send(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(await Count<Clock>(), Is.EqualTo(2));
    }

    [Test]
    public async Task FailsWhenConfirmationTokenDoesNotExist()
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

        await Add(otherEmployee);
        await Add(job);
        await Add(myAssignment);

        var request = new SubmitConfirmedClockCommand
        {
            AssignmentId = myAssignment.Id,
            ClockType = ClockType.In,
            Coordinates = coordinates,
            ConfirmationTokenValue = "who knows?"
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenConfirmationTokenHasExpired()
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
            Clocks = {new Clock {Id = Guid.NewGuid(), Type = ClockType.In, Coordinates = coordinates}}
        };

        var confirmationToken = new ConfirmationToken
        {
            Id = Guid.NewGuid(),
            Value = "confirmation",
            ExpiresAt = DateTime.UtcNow.AddDays(-1),
            ConfirmeeAssignmentId = myAssignment.Id,
            ConfirmerAssignmentId = otherAssignment.Id
        };

        await Add(otherEmployee);
        await Add(job);
        await Add(myAssignment, otherAssignment);
        await Add(confirmationToken);

        var request = new SubmitConfirmedClockCommand
        {
            AssignmentId = myAssignment.Id,
            ClockType = ClockType.In,
            Coordinates = coordinates,
            ConfirmationTokenValue = confirmationToken.Value
        };

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }
}
