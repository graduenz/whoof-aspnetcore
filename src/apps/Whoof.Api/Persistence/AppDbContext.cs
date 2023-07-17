using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Whoof.Application.Common.Interfaces;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Api.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<PetVaccination> PetVaccinations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Pet>(b =>
        {
            b.HasKey(m => m.Id);
            b.Property(m => m.CreatedAt).IsRequired();
            b.Property(m => m.Name).IsRequired();
            b.Property(m => m.PetType)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<PetType>());
        });
        
        modelBuilder.Entity<Vaccine>(b =>
        {
            b.HasKey(m => m.Id);
            b.Property(m => m.CreatedAt).IsRequired();
            b.Property(m => m.Name).IsRequired();
            b.Property(m => m.PetType)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<PetType>());
            b.Property(m => m.Duration).IsRequired();
        });
        
        modelBuilder.Entity<PetVaccination>(b =>
        {
            b.HasKey(m => m.Id);
            b.Property(m => m.CreatedAt).IsRequired();
            b.Property(m => m.PetId).IsRequired();
            b.Property(m => m.VaccineId).IsRequired();
            b.Property(m => m.AppliedAt).IsRequired();
        });
    }
}
