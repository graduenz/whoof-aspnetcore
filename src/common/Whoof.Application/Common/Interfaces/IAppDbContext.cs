using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Whoof.Domain.Entities;

namespace Whoof.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Pet> Pets { get; }
    DbSet<Vaccine> Vaccines { get; }
    DbSet<Domain.Entities.PetVaccination> PetVaccinations { get; }
    
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}