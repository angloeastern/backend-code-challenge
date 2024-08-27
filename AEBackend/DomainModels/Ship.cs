using NetTopologySuite.Geometries;


namespace AEBackend.DomainModels;
public class Ship
{
  public Knot Velocity { get; set; } = Knot.Zero();
  public double Lat { get; set; } = 0;
  public double Long { get; set; } = 0;

  public TimeSpan EstimatedArrivalTimeTo(Port port)
  {
    Point shipLocation = new Point(new Coordinate(Lat, Long));

    var meterDistance = port.GetDistance(shipLocation);

    var arrivalTime = Velocity.ArrivalTimeTo(meterDistance);

    return arrivalTime;
  }
}