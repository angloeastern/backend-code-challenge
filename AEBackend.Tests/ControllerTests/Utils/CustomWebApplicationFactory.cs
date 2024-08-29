using AEBackend;
using AEBackend.Repositories.RepositoryUsingEF;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly PostgreSqlContainer _dbContainer;

  public CustomWebApplicationFactory()
  {
    _dbContainer = new PostgreSqlBuilder().WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432)).WithImage("postgis/postgis:12-3.0").WithReuse(true).Build();
  }

  public Task InitializeAsync()
  {
    return _dbContainer.StartAsync();
  }

  async Task IAsyncLifetime.DisposeAsync()
  {
    await _dbContainer.StopAsync();
  }



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

    // builder.UseEnvironment("Development");

  }

}