using System.ComponentModel.DataAnnotations;

namespace AEBackend.DTOs;

public class CreateUserRequest
{
  [Required(ErrorMessage = "First name is required")]
  public string FirstName { get; set; } = String.Empty;

  [Required(ErrorMessage = "Last name is required")]
  public string LastName { get; set; } = String.Empty;

  [Required(ErrorMessage = "Role is required")]
  public string Role { get; set; } = String.Empty;


  [Required(ErrorMessage = "Password is required")]
  public string Password { get; set; } = String.Empty;


  [EmailAddress]
  [Required(ErrorMessage = "Email is required")]
  public string Email { get; set; } = String.Empty;

}
