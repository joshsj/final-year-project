using Microsoft.EntityFrameworkCore;
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
            .HasOne(x => x.ConfirmeeAssignment)
            .WithMany()
            .HasForeignKey(x => x.ConfirmeeAssignmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ConfirmerAssignment)
            .WithMany()
            .HasForeignKey(x => x.ConfirmerAssignmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
