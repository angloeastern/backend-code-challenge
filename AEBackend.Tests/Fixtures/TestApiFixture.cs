using System.Data.Common;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace AEBackend.Tests.Fixtures;

public class TestApiFixture : WebApplicationFactory<Program>
{
  public TestApiFixture()
  {

  }


  override protected void ConfigureWebHost(IWebHostBuilder builder)
  {

    builder.ConfigureTestServices(services =>
    {
      var dbContextDescriptor = services.SingleOrDefault(
      d => d.ServiceType ==
          typeof(DbContextOptions<UserDBContext>));

      services.Remove(dbContextDescriptor!);

      var dbConnectionDescriptor = services.SingleOrDefault(
                      d => d.ServiceType ==
                          typeof(DbConnection));

      services.Remove(dbConnectionDescriptor!);

      // Create open SqliteConnection so EF won't automatically close it.
      services.AddSingleton<DbConnection>(container =>
      {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        return connection;
      });

      services.AddDbContext<UserDBContext>((container, options) =>
      {
        var connection = container.GetRequiredService<DbConnection>();
        options.UseSqlite(connection);
      });

    });

    builder.UseEnvironment("Development");



  }

}