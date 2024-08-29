using System.Drawing;
using AEBackend.DomainModels;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace AEBackend.Repositories.RepositoryUsingEF;
public class ShipRepositoryUsingEF
{
  private readonly AppDBContext _AppDBContext;
  private readonly ILogger _logger;

  public ShipRepositoryUsingEF(AppDBContext AppDBContext, ILogger<UserRepositoryUsingEF> logger)
  {
    _AppDBContext = AppDBContext;
    _logger = logger;
  }

  public Task<bool> IsShipNameExists(string name)
  {
    var isExists = _AppDBContext.Ships.Any(s => s.Name == name);

    return Task.FromResult(isExists);
  }

  public async Task CreateShip(Ship ship)
  {
    _AppDBContext.Ships.Add(ship);
    await _AppDBContext.SaveChangesAsync();
  }

  public Task<List<Ship>> GetAllShips()
  {
    return _AppDBContext.Ships.ToListAsync();
  }

  public async Task<Ship?> UpdateShipVelocity(string id, double newVelocity)
  {
    var existingShip = await _AppDBContext.Ships.Where(x => x.Id == id).SingleOrDefaultAsync();
    if (existingShip == null)
    {
      return null;
    }

    existingShip.Velocity = new Knot(newVelocity);
    _AppDBContext.Update(existingShip);
    await _AppDBContext.SaveChangesAsync();

    return existingShip;
  }

  public async Task<List<Ship>> RetrieveShipsByIds(string[] shipIds)
  {
    var existingShips = await _AppDBContext.Ships.Where(ship => shipIds.Contains(ship.Id)).ToListAsync();

    return existingShips;
  }

  public async Task<List<Ship>> GetUnassigneds()
  {
    var unassigneds = await _AppDBContext.Ships.Include(s => s.UserShips).Where(s => s.UserShips == null || s.UserShips.Count <= 0).ToListAsync();

    return unassigneds;
  }

  public async Task<Ship?> GetShipById(string shipId)
  {
    var ship = await _AppDBContext.Ships.Where(x => x.Id == shipId).SingleOrDefaultAsync();

    return ship;
  }

  public async Task<Port?> GetNearestPort(string shipId)
  {
    var ship = await GetShipById(shipId);

    var closestPort = _AppDBContext.Ports.OrderBy(p => p.Location!.Distance(ship.Location)).FirstOrDefault();

    return closestPort;

  }
}
