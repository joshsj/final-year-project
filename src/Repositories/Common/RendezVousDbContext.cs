using Microsoft.EntityFrameworkCore;

namespace RendezVous.Repositories.Common;

public class RendezVousDbContext : DbContext
{
    public RendezVousDbContext(DbContextOptions<RendezVousDbContext> dbContextOptions)
      : base(dbContextOptions) { }
}