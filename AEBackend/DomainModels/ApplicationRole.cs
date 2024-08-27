using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AEBackend.DomainModels;

public class ApplicationRole : IdentityRole
{
  [JsonIgnore]
  public override string NormalizedName { get; set; }

  [JsonIgnore]
  public override string ConcurrencyStamp { get; set; }
  [JsonIgnore]
  public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
}