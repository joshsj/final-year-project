using Microsoft.EntityFrameworkCore;

namespace RendezVouz.Repositories.Common;

public class RendezVousDbContext : DbContext
{
    public RendezVousDbContext(DbContextOptions<RendezVousDbContext> dbContextOptions)
      : base(dbContextOptions) { }
}