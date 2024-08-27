using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AEBackend.DomainModels;

public class User : IdentityUser
{

  [JsonIgnore]
  public override string? PasswordHash { get; set; }

  [JsonIgnore]
  public override string? NormalizedEmail { get; set; }

  [JsonIgnore]
  public override string? NormalizedUserName { get; set; }

  [JsonIgnore]
  public override string? SecurityStamp { get; set; }

  [JsonIgnore]
  public override string? ConcurrencyStamp { get; set; }

  [JsonIgnore]
  public override bool LockoutEnabled { get; set; }

  [JsonIgnore]
  public override int AccessFailedCount { get; set; }

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