using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Models;
using RendezVous.Domain.Options;

namespace RendezVous.Infrastructure.Persistence;

public class RendezVousDbContextSeeder
{
    private readonly RendezVousDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly SeedOptions _seedOptions;

    public RendezVousDbContextSeeder(
        RendezVousDbContext dbContext,
        IOptions<SeedOptions> seedOptions,
        IDateTime dateTime)
    {
        _dbContext = dbContext;
        _seedOptions = seedOptions.Value;
        _dateTime = dateTime;
    }

    public async Task Wipe(CancellationToken ct = new())
    {
        // TODO change to DbSet<>
        var entityTypes = new[]
        {
            _dbContext.Locations.EntityType, _dbContext.Jobs.EntityType, _dbContext.Assignments.EntityType,
            _dbContext.Clocks.EntityType
        };

        foreach (var entityType in entityTypes)
        {
            await _dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {entityType.GetTableName()}", ct);
        }
    }

    public async Task Seed(CancellationToken ct = new())
    {
        string Lines(params string[] s) => string.Join(Environment.NewLine, s);

        var now = _dateTime.Now;

        var location = new Location
        {
            Id = Guid.NewGuid(),
            Title = "Tank Nightclub",
            Coordinates = _seedOptions.Coordinates,
            Radius = new Distance(25)
        };
        await _dbContext.Locations.AddAsync(location, ct);

        var job1 = new Job
        {
            Id = Guid.NewGuid(),
            Title = "Sammy Virji, Interplanetary Criminal & more!",
            Description = Lines(
                "The Sammy Virji residency continues with another huge line up!",
                "This time INTERPLANETARY CRIMINAL steps up on special guest duties.",
                "Along with JAMIE DUGGAN & more still TBA!"),
            Start = now.AddMinutes(-5),
            End = now.AddMinutes(60),
            LocationId = location.Id
        };
        var job2 = new Job
        {
            Id = Guid.NewGuid(),
            Title = "Andy C ft Tonn Piper & guests! X03.0 UK Tour",
            Description = Lines("ANDY C makes his first ever appearance at TANK and we cannot wait!"),
            Start = now.AddMinutes(-10),
            End = now.AddMinutes(90),
            LocationId = location.Id
        };
        await _dbContext.Jobs.AddRangeAsync(new[] {job1, job2}, ct);

        var yourAssignment = new Assignment
        {
            Id = Guid.NewGuid(),
            EmployeeId = _seedOptions.YourEmployeeId,
            JobId = job1.Id,
            Notes = "Probably on bar 1"
        };
        var otherAssignment = new Assignment
        {
            Id = Guid.NewGuid(), EmployeeId = _seedOptions.OtherEmployeeId, JobId = job1.Id, Notes = "On bar 1"
        };
        await _dbContext.Assignments.AddRangeAsync(
            new[] {yourAssignment, otherAssignment},
            ct);

        await _dbContext.SaveChangesAsync(ct);
    }
}
