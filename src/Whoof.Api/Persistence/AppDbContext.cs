using Microsoft.EntityFrameworkCore;
using Whoof.Api.Entities;

namespace Whoof.Api.Persistence;

public class AppDbContext : DbContext
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
            b.Property(m => m.PetType).IsRequired();
        });
        
        modelBuilder.Entity<Vaccine>(b =>
        {
            b.HasKey(m => m.Id);
            b.Property(m => m.CreatedAt).IsRequired();
            b.Property(m => m.Name).IsRequired();
            b.Property(m => m.PetType).IsRequired();
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
