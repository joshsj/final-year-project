using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendezVous.Domain.Entities;

namespace RendezVous.Infrastructure.Persistence.Configurations;

public class JobConfiguration : EntityConfiguration<Job>
{
    public override void Configure(EntityTypeBuilder<Job> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Start)
            .IsRequired();
        
        builder.Property(x => x.End)
            .IsRequired();

        builder
            .HasMany(x => x.Assignments)
            .WithOne(x => x.Job)
            .HasForeignKey(x => x.JobId)
            .IsRequired();
    }
}
