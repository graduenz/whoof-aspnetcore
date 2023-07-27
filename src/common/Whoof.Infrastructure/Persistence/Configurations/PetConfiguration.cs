using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Infrastructure.Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.PetType)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<PetType>());
    }
}