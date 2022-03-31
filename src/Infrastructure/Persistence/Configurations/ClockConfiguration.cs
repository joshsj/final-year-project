using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Enums;

namespace RendezVous.Infrastructure.Persistence.Configurations;

public class ClockConfiguration : EntityConfiguration<Clock>
{
    public override void Configure(EntityTypeBuilder<Clock> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.At)
            .IsRequired();

        builder.OwnsOne(x => x.Coordinates).WithOwner();

        builder
            .HasMany(x => x.Children)
            .WithOne(x => x.Parent)
            .HasForeignKey(x => x.ParentId);
    }
}
