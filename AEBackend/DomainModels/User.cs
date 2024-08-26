using Microsoft.AspNetCore.Identity;

namespace AEBackend;

public class User : IdentityUser
{
  public required string FirstName { get; set; }
  public required string LastName { get; set; }


}