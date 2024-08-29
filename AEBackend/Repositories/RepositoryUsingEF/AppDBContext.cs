using AEBackend.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AEBackend.Repositories.RepositoryUsingEF
{
  public class AppDBContext(DbContextOptions options) : IdentityDbContext<User>(options)
  {
    public DbSet<Port> Ports { get; set; }
    public DbSet<Ship> Ships { get; set; }

    public DbSet<UserShip> UserShips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // modelBuilder.Entity<Port>().Property(c => c.Location).HasSrid(4326);


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

        b.HasMany(e => e.UserShips)
              .WithOne(e => e.User)
              .HasForeignKey(ur => ur.UserId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired(false);

      });

      modelBuilder.Entity<Port>();

      modelBuilder.Entity<Ship>(s =>
      {
        s.Property(s => s.Name).IsRequired();
        s.HasMany(e => e.UserShips)
          .WithOne(e => e.Ship)
          .HasForeignKey(ur => ur.ShipId)
          .OnDelete(DeleteBehavior.Cascade)
          .IsRequired(false);

        s.OwnsOne(x => x.Velocity);
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
