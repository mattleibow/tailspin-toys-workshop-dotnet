---
description: 'xUnit test patterns and conventions for ASP.NET Core'
applyTo: '**/*Tests*/**/*.cs'
---

# .NET Test Instructions

## Test Structure

Use xUnit with WebApplicationFactory for integration tests.

### Test Class Pattern

```csharp
public class TestGamesRoutes : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    // Test data as class constants
    private static readonly Dictionary<string, object>[] TestData = [...];

    public TestGamesRoutes()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace with in-memory database
                    services.AddDbContext<TailspinToysContext>(options =>
                        options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));
                });
            });

        _client = _factory.CreateClient();
        // Seed test data
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
```

### Test Naming

- `MethodName_Scenario_ExpectedResult` pattern
- Use descriptive names that explain the test

### Assertions

```csharp
Assert.Equal(HttpStatusCode.OK, response.StatusCode);
Assert.NotNull(data);
Assert.Equal(expected, actual);
Assert.Contains(collection, item => item.Property == value);
```

### Running Tests

```bash
# Run all server tests
dotnet test server/TailspinToys.Api.Tests/

# Run with verbose output
dotnet test --verbosity normal
```
