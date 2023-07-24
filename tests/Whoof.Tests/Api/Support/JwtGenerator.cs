using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Whoof.Tests.Api.Support;

public static class JwtGenerator
{
    private static readonly Lazy<IConfigurationRoot> Configuration = new(() =>
        new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.Testing.json")
        .AddEnvironmentVariables()
        .Build());
    
    public static string GenerateBasicJwt()
    {
        var hmacSecretKey = Configuration.Value["TestAuth:HmacSecretKey"];
        
        if (string.IsNullOrEmpty(hmacSecretKey))
            throw new ArgumentException("Missing test auth HMAC secret key in settings");
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(hmacSecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("unique_id", "test@whoof.api")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}