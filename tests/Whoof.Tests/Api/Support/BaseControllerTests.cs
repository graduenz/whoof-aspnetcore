using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Whoof.Api.Persistence;
using Whoof.Tests.Data;

namespace Whoof.Tests.Api.Support;

public abstract class BaseControllerTests : IDisposable
{
    protected BaseControllerTests()
    {
        ExclusiveDbName = $"whoof_{Guid.NewGuid()}";
        TestWebApplicationFactory<Program> factory = new(ExclusiveDbName);
        
        HttpClient = factory.CreateClient();
        ServiceScope = factory.Services.CreateScope();
        DbContext = ServiceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        InitializeDatabase();
    }

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

    public string ExclusiveDbName { get; }
    protected HttpClient HttpClient { get; }
    protected IServiceScope ServiceScope { get; }
    protected AppDbContext DbContext { get; }
    protected IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;

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