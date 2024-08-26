using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AEBackend.DomainModels;

public class User : IdentityUser
{
  [Required(ErrorMessage = "First name is required")]
  public required string FirstName { get; set; }

  [Required(ErrorMessage = "Last name is required")]
  public required string LastName { get; set; }


}