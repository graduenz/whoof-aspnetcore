using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Whoof.Api.Auth;
using Whoof.Api.Services;
using Whoof.Application.Common.Interfaces;

namespace Whoof.Api;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration) => services
        .AddAuth(configuration)
        .AddScoped<ICurrentUserService, CurrentUserService>();
    
    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration["Auth0:Domain"])
            || string.IsNullOrEmpty(configuration["Auth0:Audience"]))
            throw new ArgumentException("Missing Auth0 configuration settings");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Auth0:Domain"];
                options.Audience = configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.Name
                };
            });

        services
            .AddSingleton<IAuthorizationHandler, HasScopeHandler>()
            .AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.Requirements.Add(new
                    HasScopeRequirement("admin", configuration["Auth0:Domain"]!)));
            });

        return services;
    }
}