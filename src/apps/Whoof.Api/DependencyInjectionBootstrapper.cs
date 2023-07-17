using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Persistence;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Validators;

namespace Whoof.Api;

public static class DependencyInjectionBootstrapper
{
    public static IServiceCollection
        AddApplicationServices(this IServiceCollection services, IConfiguration configuration) => services
        .AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("AppDbContext"))
        )
        .AddScoped<IAppDbContext>(provider =>
            provider.GetService<AppDbContext>() ?? throw new Exception($"Couldn't resolve {nameof(AppDbContext)}"))
        .AddValidatorsFromAssemblyContaining(typeof(PetValidator));
}