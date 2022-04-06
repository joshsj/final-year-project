using RendezVous.Application.Clocks.Commands.CreateClockConfirmationCode;
using static RendezVous.Application.IntegrationTests.Testing;

namespace RendezVous.Application.IntegrationTests.Clocks.Commands;

// rolls off the tongue
public class CreateClockConfirmationCodeCommandTests : UserTestBase
{
    [Test]
    public async Task PassesWhenRequestIsValid()
    {
        var confirmee = new Employee
        {
            Id = Guid.NewGuid(), ProviderId = CreateProviderId(), Name = "confirmee", Email = "confirmee@email.com",
        };
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Location = new Location {Id = Guid.NewGuid(), Title = ""}
        };
        var confirmerAssignment = new Assignment
        {
            Id = Guid.NewGuid(),
            EmployeeId = CurrentUser.Id,
            JobId = job.Id,
            Notes = "",
            // clocked in
            Clocks = {new() {Id = Guid.NewGuid(), Type = ClockType.In,}}
        };
        var confirmeeAssignment = new Assignment
        {
            Id = Guid.NewGuid(), EmployeeId = confirmee.Id, JobId = job.Id, Notes = "",
        };

        await Add(confirmee);
        await Add(job);
        await Add(confirmeeAssignment, confirmerAssignment);

        var request = new CreateClockConfirmationCodeCommand {ConfirmeeAssignmentId = confirmeeAssignment.Id};
        var result = await Send(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.SvgSource, Does.StartWith("<svg"));
        Assert.That(result.TimeRemaining, Is.EqualTo(TimeSpan.FromMinutes(10).TotalSeconds));
    }

    [Test]
    public void FailsWhenConfirmeeAssignmentIdIsInvalid()
    {
        var request = new CreateClockConfirmationCodeCommand {ConfirmeeAssignmentId = Guid.Empty};

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public void FailsWhenConfirmeeAssignmentDoesNotExist()
    {
        var request = new CreateClockConfirmationCodeCommand {ConfirmeeAssignmentId = Guid.NewGuid()};

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenConfirmingOwnClock()
    {
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Location = new Location {Id = Guid.NewGuid(), Title = ""}
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            // confirmer
            EmployeeId = CurrentUser.Id,
        };
        await Add(job);
        await Add(assignment);

        var request = new CreateClockConfirmationCodeCommand {ConfirmeeAssignmentId = assignment.Id};
        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenConfirmerIsNotAssignedToJob()
    {
        var confirmee = new Employee
        {
            Id = Guid.NewGuid(), ProviderId = CreateProviderId(), Name = "confirmee", Email = "confirmee@email.com",
        };
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Location = new Location {Id = Guid.NewGuid(), Title = ""}
        };
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Notes = "",
            JobId = job.Id,
            // confirmee
            EmployeeId = confirmee.Id,
        };
        await Add(confirmee);
        await Add(job);
        await Add(assignment);

        var request = new CreateClockConfirmationCodeCommand
        {
            // unknown assignment
            ConfirmeeAssignmentId = Guid.NewGuid()
        };
        
        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenConfirmerIsNotClockedIn()
    {
        var confirmee = new Employee
        {
            Id = Guid.NewGuid(), ProviderId = CreateProviderId(), Name = "confirmee", Email = "confirmee@email.com",
        };
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Location = new Location {Id = Guid.NewGuid(), Title = ""}
        };
        var confirmerAssignment = new Assignment
        {
            Id = Guid.NewGuid(),
            EmployeeId = CurrentUser.Id,
            JobId = job.Id,
            Notes = "",
            // clocked in
            Clocks = {new() {Id = Guid.NewGuid(), Type = ClockType.In,}}
        };
        var confirmeeAssignment = new Assignment
        {
            Id = Guid.NewGuid(), EmployeeId = confirmee.Id, JobId = job.Id, Notes = "",
            Clocks =
            {
                new () {Id = Guid.NewGuid(), Type = ClockType.In},
                // clocked out, no confirmations needed
                new () {Id = Guid.NewGuid(), Type = ClockType.Out}
            }
        };

        await Add(confirmee);
        await Add(job);
        await Add(confirmeeAssignment, confirmerAssignment);

        var request = new CreateClockConfirmationCodeCommand {ConfirmeeAssignmentId = confirmeeAssignment.Id};

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }

    [Test]
    public async Task FailsWhenConfirmeeIsClockedOut()
    {
        var confirmee = new Employee
        {
            Id = Guid.NewGuid(), ProviderId = CreateProviderId(), Name = "confirmee", Email = "confirmee@email.com",
        };
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Location = new Location {Id = Guid.NewGuid(), Title = ""}
        };
        var confirmerAssignment = new Assignment
        {
            Id = Guid.NewGuid(),
            EmployeeId = CurrentUser.Id,
            JobId = job.Id,
            Notes = "",
            // not clocked in
        };
        var confirmeeAssignment = new Assignment
        {
            Id = Guid.NewGuid(), EmployeeId = confirmee.Id, JobId = job.Id, Notes = "",
        };

        await Add(confirmee);
        await Add(job);
        await Add(confirmeeAssignment, confirmerAssignment);

        var request = new CreateClockConfirmationCodeCommand {ConfirmeeAssignmentId = confirmeeAssignment.Id};

        Assert.ThrowsAsync<ValidationException>(() => Send(request));
    }
}
