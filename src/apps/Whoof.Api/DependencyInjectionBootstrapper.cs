using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Persistence;
using Whoof.Api.Validators;

namespace Whoof.Api;

public static class DependencyInjectionBootstrapper
{
    public static IServiceCollection
        AddApplicationServices(this IServiceCollection services, IConfiguration configuration) => services
        .AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("AppDbContext"))
        )
        .AddValidatorsFromAssemblyContaining(typeof(PetValidator));
}