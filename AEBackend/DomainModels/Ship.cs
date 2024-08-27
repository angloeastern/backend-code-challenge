using NetTopologySuite.Geometries;


namespace AEBackend.DomainModels;
public class Ship
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public Knot Velocity { get; set; } = Knot.Zero();
  public double Lat { get; set; } = 0;
  public double Long { get; set; } = 0;

  public ICollection<UserShip> UserShips { get; set; } = [];

  public TimeSpan EstimatedArrivalTimeTo(Port port)
  {
    Point shipLocation = new Point(new Coordinate(Lat, Long));

    var meterDistance = port.GetDistance(shipLocation);

    var arrivalTime = Velocity.ArrivalTimeTo(meterDistance);

    return arrivalTime;
  }

}