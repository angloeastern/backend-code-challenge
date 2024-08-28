using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AEBackend.DomainModels;

public class ApplicationRole : IdentityRole
{
  [JsonIgnore]
  public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
}