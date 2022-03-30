using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendezVous.Domain.Entities;

namespace RendezVous.Infrastructure.Persistence.Configurations;

public class ClockConfiguration : EntityConfiguration<Clock>
{
    public override void Configure(EntityTypeBuilder<Clock> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.ExpectedAt)
            .IsRequired();

        builder.Property(x => x.ActualAt);

        builder
            .HasMany(x => x.Children)
            .WithOne(x => x.Parent)
            .HasForeignKey(x => x.ParentId);
    }
}
