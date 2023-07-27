using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Whoof.Infrastructure.Persistence;
using Whoof.Tests.Data;

namespace Whoof.Tests.Api.Support;

public abstract class BaseControllerTests : IDisposable
{
    private static JsonSerializerOptions BuildJsonOptions()
    {
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        return jsonOptions;
    }
    
    protected BaseControllerTests()
    {
        JsonOptions = BuildJsonOptions();
        
        ExclusiveDbName = $"whoof_{Guid.NewGuid()}";
        TestWebApplicationFactory<Program> factory = new(ExclusiveDbName);
        
        HttpClient = factory.CreateClient();
        ServiceScope = factory.Services.CreateScope();
        DbContext = ServiceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        InitializeDatabase();
    }

    public JsonSerializerOptions JsonOptions { get; }
    public string ExclusiveDbName { get; }
    protected HttpClient HttpClient { get; }
    protected IServiceScope ServiceScope { get; }
    protected AppDbContext DbContext { get; }
    protected IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;

    private void InitializeDatabase()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
        PreloadedData.Load(DbContext);
    }

    private void TeardownDatabase()
    {
        DbContext.Database.EnsureDeleted();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            TeardownDatabase();
            HttpClient.Dispose();
            ServiceScope.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}