using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Whoof.Api;
using Whoof.Application.Common.Dto;
using Whoof.Domain.Common;
using Whoof.Infrastructure;
using Whoof.Infrastructure.Adapters;
using Whoof.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"))
    .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Logger.Information("Initializing {App}", Assembly.GetExecutingAssembly().GetName().Name);

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
    
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Whoof.Api.xml"));
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Whoof.Infrastructure.xml"));
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Whoof.Application.xml"));
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Whoof.Domain.xml"));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddHttpContextAccessor()
    .AddSwaggerExamplesFromAssemblies(
        typeof(Program).Assembly,
        typeof(FilterAdapter).Assembly,
        typeof(BaseReadDto).Assembly,
        typeof(BaseEntity).Assembly)
    .AddInfrastructure(builder.Configuration)
    .AddApi(builder.Configuration, builder.Environment);

builder.Host.UseSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    Log.Logger.Information("Database has been successfully migrated");
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(c => c
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
    protected Program()
    {
    }
}