using Microsoft.EntityFrameworkCore;
using RendezVous.Domain.Entities;

namespace RendezVous.Application.Common.Interfaces;

public interface IRendezVousDbContext
{
    DbSet<Employee> Employees { get; }
    DbSet<Location> Locations { get; }
    DbSet<Job> Jobs { get; }
    DbSet<Assignment> Assignments { get; }
    DbSet<Clock> Clocks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
