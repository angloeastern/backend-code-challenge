
using AEBackend.Repositories.RepositoryUsingEF;

namespace AEBackend.Tests;
public class UserRepositoryUsingEFTest : IClassFixture<TestDatabaseFixture>
{
  public UserRepositoryUsingEFTest(TestDatabaseFixture fixture)
          => Fixture = fixture;

  public TestDatabaseFixture Fixture { get; }

  [Fact]
  public void CreateUser()
  {
    // using var context = Fixture.CreateUserDBContext();
    // var userRepo = new UserRepositoryUsingEF(context);

  }
}