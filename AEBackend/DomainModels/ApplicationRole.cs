using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class ApplicationRole : IdentityRole
{
  [JsonIgnore]
  public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}