﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendezVous.Domain.Entities;

namespace RendezVous.Infrastructure.Persistence.Configurations;

public class AssignmentConfiguration : EntityConfiguration<Assignment>
{
    public override void Configure(EntityTypeBuilder<Assignment> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Notes)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .HasMany(x => x.Clocks)
            .WithOne(x => x.Assignment)
            .HasForeignKey(x => x.AssignmentId)
            .IsRequired();
    }
}
