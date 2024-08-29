using System.Text.Json;
using AEBackend.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AEBackend.Repositories.RepositoryUsingEF;
public class UserRepositoryUsingEF : IUserRepository
{
  private readonly AppDBContext _AppDBContext;
  private readonly ILogger _logger;

  public UserRepositoryUsingEF(AppDBContext AppDBContext, ILogger<UserRepositoryUsingEF> logger)
  {
    _AppDBContext = AppDBContext;
    _logger = logger;
  }
  public async Task<User?> CreateUser(User user, string role)
  {
    var assignedRole = await _AppDBContext.Roles.Where(x => x.Name == role).SingleOrDefaultAsync();

    user.UserRoles = [
      new ApplicationUserRole{
        RoleId = assignedRole.Id,
        UserId = user.Id
      }
    ];

    _AppDBContext.Users.Add(user);
    await _AppDBContext.SaveChangesAsync();

    return await _AppDBContext.Users.Where(x => x.Id == user.Id).SingleOrDefaultAsync();
  }

  public async Task<List<User>> GetAllUsers()
  {
    _logger.LogDebug("Calling _AppDBContext.Users.Include...");
    try
    {
      var allUsers = await _AppDBContext.Users.Include(u => u.UserRoles)!.ThenInclude(ur => ur.Role)
            .Include(u => u.UserShips).ThenInclude(x => x.Ship).ToListAsync();

      _logger.LogDebug("Calling _AppDBContext.Users.Include...[DONE] {0}", JsonSerializer.Serialize(allUsers));

      return allUsers;

    }
    catch (System.Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }

  public Task<User?> GetUserById(string id)
  {
    return _AppDBContext.Users.Include(x => x.UserRoles).ThenInclude(ur => ur.Role).Include(u => u.UserShips).ThenInclude(s => s.Ship).Where(x => x.Id == id).SingleOrDefaultAsync();
  }

  public async Task<List<Ship>> GetUserShips(string userId)
  {
    var userShips = await _AppDBContext.Users.Include(u => u.UserShips).ThenInclude(us => us.Ship).Where(x => x.Id == userId).SingleAsync();

    return userShips.UserShips.Select(x => x.Ship).ToList();
  }

  public async Task<User> UpdateUserShips(User existingUser, string[] shipdIds)
  {

    var updatingUser = await GetUserById(existingUser.Id);

    _AppDBContext.RemoveRange(updatingUser.UserShips);
    await _AppDBContext.SaveChangesAsync();


    shipdIds.ToList().ForEach(shipId =>
    {
      var newUserShip = new UserShip() { ShipId = shipId, UserId = existingUser.Id };

      updatingUser.UserShips.Add(newUserShip);
    });

    await _AppDBContext.SaveChangesAsync();

    var updatedUser = await GetUserById(existingUser.Id);

    return updatedUser;
  }
}