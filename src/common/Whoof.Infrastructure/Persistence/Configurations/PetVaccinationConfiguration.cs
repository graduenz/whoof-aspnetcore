using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Whoof.Infrastructure.Persistence.Configurations;

public class PetVaccinationConfiguration : IEntityTypeConfiguration<Domain.Entities.PetVaccination>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.PetVaccination> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.PetId).IsRequired();
        builder.Property(m => m.VaccineId).IsRequired();
        builder.Property(m => m.AppliedAt).IsRequired();
    }
}