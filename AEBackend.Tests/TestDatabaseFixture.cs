using AEBackend;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.EntityFrameworkCore;

public class TestDatabaseFixture
{
  private const string ConnectionString = @"User ID =postgres;Password=010679;Server=app_db;Port=5432;Database=SampleDriverDb; Integrated Security=true;Pooling=true;";

  private static readonly object _lock = new();
  private static bool _databaseInitialized;

  public TestDatabaseFixture()
  {
    lock (_lock)
    {
      if (!_databaseInitialized)
      {
        using (var context = CreateUserDBContext())
        {
          context.Database.EnsureDeleted();
          context.Database.EnsureCreated();

          context.SaveChanges();
        }

        _databaseInitialized = true;
      }
    }
  }

  public UserDBContext CreateUserDBContext()
  {
    return new UserDBContext(new DbContextOptionsBuilder<UserDBContext>().UseNpgsql(ConnectionString).Options);
  }
}