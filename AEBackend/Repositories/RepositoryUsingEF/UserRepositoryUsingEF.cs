using System.Text.Json;
using AEBackend.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AEBackend.Repositories.RepositoryUsingEF;
public class UserRepositoryUsingEF : IUserRepository
{
  private readonly UserDBContext _userDBContext;
  private readonly ILogger _logger;

  public UserRepositoryUsingEF(UserDBContext userDBContext, ILogger<UserRepositoryUsingEF> logger)
  {
    _userDBContext = userDBContext;
    _logger = logger;
  }
  public async Task CreateUser(User user)
  {

    await _userDBContext.Users.AddAsync(user);
    // await _userDBContext.SaveChangesAsync();

    var role = AppRoles.Get(user.UserRoles.First()!.Role.Name!)!;

    ApplicationUserRole identityUserRole = new()
    {
      RoleId = role.Id,
      UserId = user.Id
    };

    await _userDBContext.UserRoles.AddAsync(identityUserRole);
    await _userDBContext.SaveChangesAsync();
  }

  public async Task<List<User>> GetAllUsers()
  {
    _logger.LogDebug("Calling _userDBContext.Users.Include...");
    try
    {
      var allUsers = await _userDBContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();

      _logger.LogDebug("Calling _userDBContext.Users.Include...[DONE] {0}", JsonSerializer.Serialize(allUsers));

      return allUsers;

    }
    catch (System.Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }
}