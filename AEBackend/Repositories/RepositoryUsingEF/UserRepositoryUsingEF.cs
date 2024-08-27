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
  public async Task CreateUser(User user)
  {

    await _AppDBContext.Users.AddAsync(user);

    var role = AppRoles.Get(user.UserRoles!.First()!.Role!.Name!)!;

    ApplicationUserRole identityUserRole = new()
    {
      RoleId = role.Id,
      UserId = user.Id
    };

    await _AppDBContext.UserRoles.AddAsync(identityUserRole);
    await _AppDBContext.SaveChangesAsync();
  }

  public async Task<List<User>> GetAllUsers()
  {
    _logger.LogDebug("Calling _AppDBContext.Users.Include...");
    try
    {
      var allUsers = await _AppDBContext.Users.Include(u => u.UserRoles)!.ThenInclude(ur => ur.Role).ToListAsync();

      _logger.LogDebug("Calling _AppDBContext.Users.Include...[DONE] {0}", JsonSerializer.Serialize(allUsers));

      return allUsers;

    }
    catch (System.Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }
}