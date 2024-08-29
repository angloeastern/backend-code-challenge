using System.ComponentModel.DataAnnotations;

namespace AEBackend.DTOs;

public class UpdateShipsAssignedToUserRequest
{
  [Required]
  public string[] ShipdIds { get; set; } = [];
}