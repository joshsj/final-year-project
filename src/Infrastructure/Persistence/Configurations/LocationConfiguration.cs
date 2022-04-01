using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendezVous.Domain.Entities;

namespace RendezVous.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : EntityConfiguration<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasMany(x => x.Jobs)
            .WithOne(x => x.Location)
            .HasForeignKey(x => x.LocationId)
            .IsRequired();

        builder.OwnsOne(x => x.Coordinates).WithOwner();
        builder.OwnsOne(x => x.Radius).WithOwner();
    }
}
