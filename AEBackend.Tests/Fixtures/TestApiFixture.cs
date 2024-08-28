
using System.Data.Common;
using AEBackend.Repositories.RepositoryUsingEF;
using AEBackend.Tests.Fixtures;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
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


public class TestApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly PostgreSqlContainer _dbContainer;
  public TestApiFixture()
  {
    _dbContainer = new PostgreSqlBuilder().WithImage("postgis/postgis:12-3.0").Build();
  }

  public Task InitializeAsync()
  {
    return _dbContainer.StartAsync();
  }

  public string DBConnectionString
  {
    get
    {
      return _dbContainer.GetConnectionString();
    }
  }

  public string ContainerId
  {
    get
    {
      return _dbContainer.Id;
    }
  }

  public new Task DisposeAsync()
  {

    return _dbContainer.StopAsync();
  }

  // private void ConfigureSqlite(IWebHostBuilder builder)
  // {
  //   // SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
  //   builder.ConfigureTestServices(services =>
  //   {
  //     var dbContextDescriptor = services.SingleOrDefault(
  //     d => d.ServiceType ==
  //         typeof(DbContextOptions<AppDBContext>));

  //     services.Remove(dbContextDescriptor!);

  //     var dbConnectionDescriptor = services.SingleOrDefault(
  //                     d => d.ServiceType ==
  //                         typeof(DbConnection));

  //     services.Remove(dbConnectionDescriptor!);

  //     // Create open SqliteConnection so EF won't automatically close it.
  //     services.AddSingleton<DbConnection>(container =>
  //     {
  //       var connection = new SqliteConnection("DataSource=:memory:");
  //       connection.Open();

  //       return connection;
  //     });

  //     services.AddDbContext<AppDBContext>((container, options) =>
  //     {
  //       // var connection = container.GetRequiredService<DbConnection>();
  //       options.UseSqlite(x => x.UseNetTopologySuite());

  //     });

  //   });
  // }

  override protected void ConfigureWebHost(IWebHostBuilder builder)
  {

    builder.ConfigureTestServices(services =>
      {
        var descriptorType =
        typeof(DbContextOptions<AppDBContext>);

        var descriptor = services
        .SingleOrDefault(s => s.ServiceType == descriptorType);

        if (descriptor is not null)
        {
          services.Remove(descriptor);
        }


        services.AddDbContext<AppDBContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString(), x => x.UseNetTopologySuite()));
      });

    builder.UseEnvironment("Development");

  }

}