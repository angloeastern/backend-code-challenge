using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AEBackend.Repositories.RepositoryUsingEF
{
  public class UserDBContext(DbContextOptions options) : IdentityDbContext<User>(options)
  {

  }
}
