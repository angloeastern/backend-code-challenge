using System.ComponentModel.DataAnnotations;

namespace AEBackend.DTOs;

public class CreateUserRequest
{
  [Required(ErrorMessage = "First name is required")]
  [StringLength(100)]
  public string FirstName { get; set; } = String.Empty;

  [Required(ErrorMessage = "Last name is required")]
  [StringLength(100)]
  public string LastName { get; set; } = String.Empty;

  [Required(ErrorMessage = "Role is required")]
  [StringLength(20)]
  public string Role { get; set; } = String.Empty;


  [Required(ErrorMessage = "Password is required")]
  [StringLength(80)]
  public string Password { get; set; } = String.Empty;


  [EmailAddress]
  [StringLength(100)]
  [Required(ErrorMessage = "Email is required")]
  public string Email { get; set; } = String.Empty;

}
