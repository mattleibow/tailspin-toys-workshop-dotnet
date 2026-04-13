using Microsoft.EntityFrameworkCore;
using TailspinToys.Api;
using TailspinToys.Api.Routes;
using TailspinToys.Api.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TailspinToysContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    options.UseSqlite(connectionString);
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Create tables and seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TailspinToysContext>();
    db.Database.EnsureCreated();

    var seedEnabled = app.Configuration.GetValue("SeedDatabase", false);
    if (seedEnabled)
    {
        SeedDatabase.Seed(db);
    }
}

app.UseCors();

// Register routes
app.MapGamesRoutes();

app.Run();

// Make Program accessible to test project
public partial class Program { }
