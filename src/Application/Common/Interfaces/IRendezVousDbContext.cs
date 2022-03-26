namespace RendezVous.Application.Common.Interfaces;

public interface IRendezVousDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
