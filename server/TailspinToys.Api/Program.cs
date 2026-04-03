using Microsoft.EntityFrameworkCore;
using TailspinToys.Api;
using TailspinToys.Api.Routes;
using TailspinToys.Api.Utils;

var builder = WebApplication.CreateBuilder(args);

// Configure the database
var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
var dataDir = Path.Combine(projectRoot, "data");
Directory.CreateDirectory(dataDir);
var dbPath = Path.Combine(dataDir, "tailspin-toys.db");

builder.Services.AddDbContext<TailspinToysContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

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
    if (db.Database.IsSqlite())
    {
        db.Database.EnsureCreated();
    }
    SeedDatabase.Seed(db);
}

app.UseCors();

// Register routes
app.MapGamesRoutes();

app.Run();

// Make Program accessible to test project
public partial class Program { }
