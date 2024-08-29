using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;


namespace AEBackend.DomainModels;
public class Ship
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public Knot Velocity { get; set; } = Knot.Zero();
  public double Lat { get; set; } = 0;
  public double Longi { get; set; } = 0;

  [JsonIgnore]
  public ICollection<UserShip> UserShips { get; set; } = [];

  public TimeSpan EstimatedArrivalTimeTo(Port port)
  {
    Point shipLocation = new Point(new Coordinate(Lat, Longi));

    var meterDistance = port.GetDistance(shipLocation);

    var arrivalTime = Velocity.ArrivalTimeTo(meterDistance);

    return arrivalTime;
  }

  [NotMapped]
  [JsonIgnore]
  public Point Location
  {
    get
    {
      return new Point(new Coordinate(Lat, Longi));
    }
  }

}