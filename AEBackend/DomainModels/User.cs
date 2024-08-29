using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AEBackend.DomainModels;

public class User : IdentityUser
{



  [PersonalData]
  public string FirstName { get; set; } = string.Empty;

  [PersonalData]
  public string LastName { get; set; } = string.Empty;

  [JsonIgnore]
  public virtual ICollection<IdentityUserClaim<string>>? Claims { get; set; }

  [JsonIgnore]
  public virtual ICollection<IdentityUserLogin<string>>? Logins { get; set; }

  [JsonIgnore]
  public virtual ICollection<IdentityUserToken<string>>? Tokens { get; set; }
  public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }

  public virtual ICollection<UserShip>? UserShips { get; set; }

}