using System.Reflection;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Common;
using Microsoft.EntityFrameworkCore;
using RendezVous.Domain.Entities;

namespace RendezVous.Infrastructure.Persistence;

public class RendezVousDbContext : DbContext, IRendezVousDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IDomainEventService _domainEventService;

    public RendezVousDbContext(
        DbContextOptions<RendezVousDbContext> options,
        ICurrentUserService currentUserService,
        IDomainEventService domainEventService,
        IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _domainEventService = domainEventService;
        _dateTime = dateTime;
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Clock> Clocks => Set<Clock>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var currentUserId = (await Employees.FirstOrDefaultAsync(
            x => x.ProviderId == _currentUserService.ProviderId,
            cancellationToken))?.Id;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.Created = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = currentUserId;
                    entry.Entity.LastModified = _dateTime.Now;
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // use entity name (e.g., Job) as table name
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.DisplayName());
        }

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}
