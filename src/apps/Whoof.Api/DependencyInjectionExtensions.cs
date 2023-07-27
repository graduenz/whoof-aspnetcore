using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Whoof.Api.Services;
using Whoof.Application.Common.Interfaces;

namespace Whoof.Api;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment) => services
        .AddAuth(configuration, environment)
        .AddScoped<ICurrentUserService, CurrentUserService>();

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        if (environment.EnvironmentName == "Testing")
            return AddTestAuth(services, configuration);

        return AddRegularAuth(services, configuration);
    }

    private static IServiceCollection AddTestAuth(IServiceCollection services, IConfiguration configuration)
    {
        var hmacSecretKey = configuration["TestAuth:HmacSecretKey"];
        
        if (string.IsNullOrEmpty(hmacSecretKey))
            throw new ArgumentException("Missing test auth HMAC secret key in settings");
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(hmacSecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        
        return services;
    }
    
    private static IServiceCollection AddRegularAuth(IServiceCollection services, IConfiguration configuration)
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

        return services;
    }
}