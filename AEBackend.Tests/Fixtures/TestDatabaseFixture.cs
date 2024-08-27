using AEBackend;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace AEBackend.Tests.Fixtures;

public class TestDatabaseFixture : IAsyncLifetime
{
  private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().WithDatabase("example")
      .WithUsername("postgres").WithPassword("010679").Build();


  private static readonly object _lock = new();
  private static bool _databaseInitialized;

  public TestDatabaseFixture()
  {
    lock (_lock)
    {
      if (!_databaseInitialized)
      {
        using (var context = CreateAppDBContext())
        {
          context.Database.EnsureDeleted();
          context.Database.EnsureCreated();

          context.SaveChanges();
        }

        _databaseInitialized = true;
      }
    }
  }

  public async Task InitializeAsync()
  {
    await _postgreSqlContainer.StartAsync();
  }

  async Task IAsyncLifetime.DisposeAsync()
  {
    await _postgreSqlContainer.StopAsync();
  }
  public AppDBContext CreateAppDBContext()
  {
    return new AppDBContext(new DbContextOptionsBuilder<AppDBContext>().UseNpgsql(_postgreSqlContainer.GetConnectionString()).Options);
  }
}