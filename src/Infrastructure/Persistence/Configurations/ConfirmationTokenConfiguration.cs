using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendezVous.Domain.Entities;

namespace RendezVous.Infrastructure.Persistence.Configurations;

public class ConfirmationTokenConfiguration : EntityConfiguration<ConfirmationToken>
{
    public override void Configure(EntityTypeBuilder<ConfirmationToken> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Value)
            .IsRequired();

        builder.HasIndex(x => x.Value)
            .IsUnique();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder
            .HasOne(x => x.Assignment)
            .WithMany()
            .HasForeignKey(x => x.AssignmentId)
            .IsRequired();
    }
}
