using AEBackend.DomainModels;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;


namespace AEBackend.Repositories.Seeder;

public class Seeder
{

  private readonly ILogger<Seeder> _logger;
  private readonly AppDBContext _appDBContext;

  public Seeder(AppDBContext appDBContext, ILogger<Seeder> logger)
  {
    _appDBContext = appDBContext;

    _logger = logger;
  }

  private async Task SeedAdminUser()
  {
    string ADMIN_ID = "203557e0-b2f4-449c-9671-e69fe5ee6d86";
    if (!_appDBContext.Users.Any(x => x.Email == "irwansyah@gmail.com"))
    {
      _logger.LogDebug("USER NOT EXISTS:" + ADMIN_ID);

      _appDBContext.Roles.Add(AppRoles.Administrator);

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

      _appDBContext.Users.Add(adminUser);

      ApplicationUserRole identityUserRole = new()
      {
        RoleId = AppRoles.Administrator.Id,
        UserId = adminUser.Id
      };

      _appDBContext.UserRoles.Add(identityUserRole);

      await _appDBContext.SaveChangesAsync();
    }

  }

  private async Task SeedUserRoles()
  {
    if (!_appDBContext.Roles.Any(x => x.Id == AppRoles.Administrator.Id))
    {
      _appDBContext.Roles.Add(new ApplicationRole
      {
        Id = AppRoles.Administrator.Id,
        Name = AppRoles.Administrator.Name,
        NormalizedName = AppRoles.Administrator.NormalizedName
      });
      await _appDBContext.SaveChangesAsync();
    }
    if (!_appDBContext.Roles.Any(x => x.Id == AppRoles.User.Id))
    {
      _appDBContext.Roles.Add(new ApplicationRole
      {
        Id = AppRoles.User.Id,
        Name = AppRoles.User.Name,
        NormalizedName = AppRoles.User.NormalizedName
      });
      await _appDBContext.SaveChangesAsync();
    }
    if (!_appDBContext.Roles.Any(x => x.Id == AppRoles.VipUser.Id))
    {
      _appDBContext.Roles.Add(new ApplicationRole
      {
        Id = AppRoles.VipUser.Id,
        Name = AppRoles.VipUser.Name,
        NormalizedName = AppRoles.VipUser.NormalizedName
      });
      await _appDBContext.SaveChangesAsync();
    }



  }

  private async Task SeedPorts()
  {
    if (!_appDBContext.Ports.Any())
    {

      var ports = PortsJsonLoader.LoadJson(_logger)!.Select(x =>
            {
              string id = x.unlocs![0];
              double lat = x.coordinates![0];
              double longi = x.coordinates![1];

              var port = new Port
              {
                Id = id,
                City = x.city!,
                Country = x.country!,
                Lat = lat,
                Long = longi,
                Location = new NetTopologySuite.Geometries.Point(new Coordinate(longi, lat)),
                Name = x.name!
              };
              return port;
            });


      foreach (var port in ports)
      {
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