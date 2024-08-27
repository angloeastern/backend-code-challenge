using AEBackend.DomainModels;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.AspNetCore.Identity;

public class Seeder
{
  private readonly UserDBContext _userDBContext;

  public Seeder(UserDBContext userDBContext)
  {
    _userDBContext = userDBContext;
  }

  private async Task SeedAdminUser()
  {
    if (!_userDBContext.Users.Any())
    {
      string ADMIN_ID = "203557e0-b2f4-449c-9671-e69fe5ee6d86";

      await _userDBContext.Roles.AddAsync(AppRoles.Administrator);

      User adminUser = new()
      {
        Id = ADMIN_ID,
        Email = "irwansyah@gmail.com",
        EmailConfirmed = true,
        FirstName = "Irwansyah",
        LastName = "Irwansyah",
        UserName = "admin",
        NormalizedUserName = "ADMIN",
        NormalizedEmail = "IRWANSYAH@GMAIL.COM",
      };

      PasswordHasher<User> ph = new();
      adminUser.PasswordHash = ph.HashPassword(adminUser, "Abcd1234!");
      await _userDBContext.Users.AddAsync(adminUser);


      ApplicationUserRole identityUserRole = new()
      {
        RoleId = AppRoles.Administrator.Id,
        UserId = ADMIN_ID
      };

      await _userDBContext.UserRoles.AddAsync(identityUserRole);
      await _userDBContext.SaveChangesAsync();
    }
  }

  private async Task SeedUserRoles()
  {
    if (!_userDBContext.Roles.Any(x => x.Name == AppRoles.Administrator.Name))
    {
      await _userDBContext.Roles.AddAsync(AppRoles.Administrator);
    }
    if (!_userDBContext.Roles.Any(x => x.Name == AppRoles.User.Name))
    {
      await _userDBContext.Roles.AddAsync(AppRoles.User);
    }
    if (!_userDBContext.Roles.Any(x => x.Name == AppRoles.VipUser.Name))
    {
      await _userDBContext.Roles.AddAsync(AppRoles.VipUser);
    }

    await _userDBContext.SaveChangesAsync();

  }

  public async Task Seed()
  {
    await SeedAdminUser();
    await SeedUserRoles();
  }
}