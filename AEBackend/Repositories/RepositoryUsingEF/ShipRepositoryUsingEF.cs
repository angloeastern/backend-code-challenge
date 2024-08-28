using AEBackend.DomainModels;
using Microsoft.EntityFrameworkCore;

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

}
