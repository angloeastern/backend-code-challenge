using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AEBackend.DomainModels;

[PrimaryKey(nameof(UserId), nameof(ShipId))]
public class UserShip
{
  public string? UserId { get; set; }

  public string? ShipId { get; set; }

  [JsonIgnore]
  public virtual User? User { get; set; }
  public virtual Ship? Ship { get; set; }

}