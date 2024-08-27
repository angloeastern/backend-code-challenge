using AEBackend.DomainModels;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;


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

      double[] pointBandung = [-6.9796439391129015, 107.72736494836637];
      double[] pointCisauk = [-6.337957856734018, 106.64177845684021];
      double[] pointKarangAnyar = [-7.603521654106759, 111.01227712457657];

      Ship ship = new Ship
      {
        Lat = pointBandung[0],
        Long = pointBandung[1],
        Velocity = new Knot(16)
      };
      Point currentLocation = new Point(new Coordinate(ship.Lat, ship.Long));



      var closestPort = _appDBContext.Ports.OrderBy(p => p.Location!.Distance(currentLocation)).FirstOrDefault();

      if (closestPort != null)
      {
        _logger.LogDebug("Closest port is:" + closestPort.Name + ", " + closestPort.City);
        _logger.LogDebug("Closest port distance:" + closestPort.GetDistance(currentLocation).Value);
        _logger.LogDebug("Closest port arrival time:" + ship.EstimatedArrivalTimeTo(closestPort));
      }
    }
  }

  public async Task Seed()
  {
    await SeedAdminUser();
    await SeedUserRoles();
    await SeedPorts();
  }
}