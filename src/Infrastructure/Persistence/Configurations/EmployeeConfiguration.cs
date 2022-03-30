using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendezVous.Domain.Entities;

namespace RendezVous.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : EntityConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.ProviderId).IsRequired();

        builder.HasIndex(x => x.ProviderId).IsUnique();

        builder
            .HasMany(x => x.Assignments)
            .WithOne(x => x.Employee)
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired();
    }
}
