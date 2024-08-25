using AEBackend;
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
      Console.WriteLine("A");
      string ADMIN_ID = "203557e0-b2f4-449c-9671-e69fe5ee6d86";
      string ROLE_ID = "4b390270-3075-4a64-814a-6f7223e921b1";

      IdentityRole adminRole = new()
      {
        Name = "Admin",
        NormalizedName = "ADMIN",
        Id = ROLE_ID,
        ConcurrencyStamp = ROLE_ID
      };

      await _userDBContext.Roles.AddAsync(adminRole);

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


      IdentityUserRole<string> identityUserRole = new()
      {
        RoleId = ROLE_ID,
        UserId = ADMIN_ID
      };

      await _userDBContext.UserRoles.AddAsync(identityUserRole);
      await _userDBContext.SaveChangesAsync();
    }

    Console.WriteLine("B");
  }

  public async Task Seed()
  {
    await SeedAdminUser();
  }
}