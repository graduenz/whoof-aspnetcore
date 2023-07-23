using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whoof.Application.AutoMapper;
using Whoof.Application.Common.Behaviors;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Pets;
using Whoof.Application.Pets.Validators;
using Whoof.Infrastructure.Adapters;
using Whoof.Infrastructure.Persistence;

namespace Whoof.Infrastructure;

public static class DependencyInjectionBootstrapper
{
    public static IServiceCollection
        AddInfrastructure(this IServiceCollection services, IConfiguration configuration) => services
        .AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("AppDbContext"))
        )
        .AddScoped<IAppDbContext>(provider =>
            provider.GetService<AppDbContext>() ?? throw new Exception($"Couldn't resolve {nameof(AppDbContext)}"))
        .AddValidatorsFromAssemblyContaining(typeof(PetDtoValidator))
        .AddAdapters()
        .AddMediatR()
        .AddAutoMapper();
    
    private static IServiceCollection AddAdapters(this IServiceCollection services) => services
        .AddScoped<IFilterAdapter, FilterAdapter>()
        .AddScoped<ISortAdapter, SortAdapter>();

    private static IServiceCollection AddMediatR(this IServiceCollection services) => services
        .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePetCommand).Assembly))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
    
    private static IServiceCollection AddAutoMapper(this IServiceCollection services) => services
        .AddAutoMapper(typeof(Domain2DtoProfile));
}