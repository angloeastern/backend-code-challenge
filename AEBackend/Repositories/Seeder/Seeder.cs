using AEBackend.DomainModels;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;


namespace AEBackend.Repositories.Seeder;

public class Seeder
{
  private readonly AppDBContext _appDBContext;
  private readonly ILogger<Seeder> _logger;

  public Seeder(AppDBContext appDBContext, ILogger<Seeder> logger)
  {
    _appDBContext = appDBContext;
    _logger = logger;
  }

  private async Task SeedAdminUser()
  {
    if (!_appDBContext.Users.Any())
    {
      string ADMIN_ID = "203557e0-b2f4-449c-9671-e69fe5ee6d86";

      await _appDBContext.Roles.AddAsync(AppRoles.Administrator);

      User adminUser = new()
      {
        Id = ADMIN_ID,
        Email = "irwansyah@gmail.com",
        EmailConfirmed = true,
        FirstName = "Irwansyah",
        LastName = "Irwansyah",
        UserName = "irwansyah@gmail.com",
        NormalizedUserName = "IRWANSYAH@GMAIL.COM",
        NormalizedEmail = "IRWANSYAH@GMAIL.COM",
      };

      PasswordHasher<User> ph = new();
      adminUser.PasswordHash = ph.HashPassword(adminUser, "Abcd1234!");
      await _appDBContext.Users.AddAsync(adminUser);


      ApplicationUserRole identityUserRole = new()
      {
        RoleId = AppRoles.Administrator.Id,
        UserId = ADMIN_ID
      };

      await _appDBContext.UserRoles.AddAsync(identityUserRole);
      await _appDBContext.SaveChangesAsync();
    }
  }

  private async Task SeedUserRoles()
  {
    if (!_appDBContext.Roles.Any(x => x.Name == AppRoles.Administrator.Name))
    {
      await _appDBContext.Roles.AddAsync(AppRoles.Administrator);
    }
    if (!_appDBContext.Roles.Any(x => x.Name == AppRoles.User.Name))
    {
      await _appDBContext.Roles.AddAsync(AppRoles.User);
    }
    if (!_appDBContext.Roles.Any(x => x.Name == AppRoles.VipUser.Name))
    {
      await _appDBContext.Roles.AddAsync(AppRoles.VipUser);
    }

    await _appDBContext.SaveChangesAsync();

  }

  private async Task SeedPorts()
  {
    if (!_appDBContext.Ports.Any())
    {

      var ports = PortsJsonLoader.LoadJson(_logger)!.Select(x =>
            {
              string id = x.unlocs![0];

              var port = new Port
              {
                Id = id,
                City = x.city!,
                Country = x.country!,
                Lat = x.coordinates![0],
                Long = x.coordinates![1],
                Name = x.name!
              };
              return port;
            });


      foreach (var port in ports)
      {
        _logger.LogDebug("AddAsync port id:" + port.Id);
        await _appDBContext.Ports.AddAsync(port);
      }

      _logger.LogDebug("Saving Ports to DB...");
      await _appDBContext.SaveChangesAsync();

    }
  }

  public async Task Seed()
  {
    await SeedAdminUser();
    await SeedUserRoles();
    await SeedPorts();
  }
}