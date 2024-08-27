using AEBackend.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AEBackend.Repositories.RepositoryUsingEF
{
  public class UserDBContext(DbContextOptions options) : IdentityDbContext<User>(options)
  {
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>(b =>
      {
        // Each User can have many UserClaims
        b.HasMany(e => e.Claims)
              .WithOne()
              .HasForeignKey(uc => uc.UserId)
              .IsRequired();

        // Each User can have many UserLogins
        b.HasMany(e => e.Logins)
              .WithOne()
              .HasForeignKey(ul => ul.UserId)
              .IsRequired();

        // Each User can have many UserTokens
        b.HasMany(e => e.Tokens)
              .WithOne()
              .HasForeignKey(ut => ut.UserId)
              .IsRequired();

        // Each User can have many entries in the UserRole join table
        b.HasMany(e => e.UserRoles)
              .WithOne(e => e.User)
              .HasForeignKey(ur => ur.UserId)
              .IsRequired();
      });

      modelBuilder.Entity<ApplicationRole>(b =>
      {
        // Each Role can have many entries in the UserRole join table
        b.HasMany(e => e.UserRoles)
              .WithOne(e => e.Role)
              .HasForeignKey(ur => ur.RoleId)
              .IsRequired();
      });

    }

  }
}
