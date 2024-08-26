using System.ComponentModel.DataAnnotations;

public class Login
{
  [Required(ErrorMessage = "Username is required")]
  public string Username { get; set; } = String.Empty;

  [Required(ErrorMessage = "Password is required")]
  public string Password { get; set; } = String.Empty;

}