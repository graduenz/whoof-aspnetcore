using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Interfaces;
using Whoof.Domain.Entities;

namespace Whoof.Infrastructure.Persistence;

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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
