using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whoof.Infrastructure.Persistence;

namespace Whoof.Tests.Api.Support;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _exclusiveDbName;

    public TestWebApplicationFactory(string exclusiveDbName)
    {
        _exclusiveDbName = exclusiveDbName;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        builder.UseConfiguration(configuration);
        
        builder.ConfigureServices(services =>
        {
            ReplaceDbConnectionString(services, configuration);
        });

        builder.UseEnvironment("Development");
    }

    private void ReplaceDbConnectionString(IServiceCollection services, IConfiguration configuration)
    {
        var dbContextDescriptor = services.Single(
            d => d.ServiceType ==
                 typeof(DbContextOptions<AppDbContext>));

        services.Remove(dbContextDescriptor);
        
        services.AddDbContext<AppDbContext>(options =>
        {
            var connstrBuilder = new DbConnectionStringBuilder();
            connstrBuilder.ConnectionString = configuration["ConnectionStrings:AppDbContext"];
            connstrBuilder["Database"] = _exclusiveDbName;
            options.UseNpgsql(connstrBuilder.ConnectionString);
        });
    }
}