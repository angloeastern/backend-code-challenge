using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AEBackend.Repositories.RepositoryUsingEF
{
  public class UserDBContext(DbContextOptions options) : IdentityDbContext<User>(options)
  {
    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //   base.OnModelCreating(builder);

    //   string ADMIN_ID = "203557e0-b2f4-449c-9671-e69fe5ee6d86";
    //   string ROLE_ID = "4b390270-3075-4a64-814a-6f7223e921b1";

    //   //seed admin role
    //   builder.Entity<IdentityRole>().HasData(new IdentityRole
    //   {
    //     Name = "Admin",
    //     NormalizedName = "ADMIN",
    //     Id = ROLE_ID,
    //     ConcurrencyStamp = ROLE_ID
    //   });

    //   //create user
    //   var appUser = new User
    //   {
    //     Id = ADMIN_ID,
    //     Email = "irwansyah@gmail.com",
    //     EmailConfirmed = true,
    //     FirstName = "Irwansyah",
    //     LastName = "Irwansyah",
    //     UserName = "irwansyah@gmail.com",
    //     NormalizedUserName = "IRWANSYAH@GMAIL.COM"
    //   };

    //   //set user password
    //   PasswordHasher<User> ph = new PasswordHasher<User>();
    //   appUser.PasswordHash = ph.HashPassword(appUser, "Abcd1234!");

    //   //seed user
    //   builder.Entity<User>().HasData(appUser);

    //   //set user role to admin
    //   builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
    //   {
    //     RoleId = ROLE_ID,
    //     UserId = ADMIN_ID
    //   });
    // }
  }
}
