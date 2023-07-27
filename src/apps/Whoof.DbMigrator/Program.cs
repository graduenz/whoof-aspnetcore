using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whoof.Infrastructure;
using Whoof.Infrastructure.Persistence;

Console.WriteLine("Loading configuration");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Testing.json")
    .AddEnvironmentVariables()
    .Build();

Console.WriteLine("Registering all dependencies");

var services = new ServiceCollection();
services.AddInfrastructure(configuration);

var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Migrating database");

var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
dbContext.Database.Migrate();

Console.WriteLine("Done!");