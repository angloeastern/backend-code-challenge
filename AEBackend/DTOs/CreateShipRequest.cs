using System.ComponentModel.DataAnnotations;

namespace AEBackend.DTOs;

public class CreateShipRequest
{
  [StringLength(100)]
  [Required]
  public string Name { get; set; } = string.Empty;

  [Range(0, 60)]
  public double KnotVelocity { get; set; } = 0;

  [Range(-90, 90)]
  public double Lat { get; set; } = 0;

  [Range(-180, 180)]
  public double Long { get; set; } = 0;

}