using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AEBackend.DTOs;

public class UpdateShipVelocityRequest
{
  [Range(0, 60)]
  public double KnotVelocity { get; set; } = 0;
}