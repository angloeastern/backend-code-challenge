using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace AEBackend.DomainModels;

public class Port
{
  public string Name { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;
  public string Id { get; set; } = string.Empty;

  public double Lat { get; set; } = 0;
  public double Long { get; set; } = 0;

  [JsonIgnore]
  public Point? Location { get; set; }

  private double toRadians(double angleIn10thofaDegree)
  {
    // Angle in 10th
    // of a degree
    return (angleIn10thofaDegree *
                   Math.PI) / 180;
  }

  private double distance(double lat1,
                 double lat2,
                 double lon1,
                 double lon2)
  {

    // The math module contains
    // a function named toRadians
    // which converts from degrees
    // to radians.
    lon1 = toRadians(lon1);
    lon2 = toRadians(lon2);
    lat1 = toRadians(lat1);
    lat2 = toRadians(lat2);

    // Haversine formula
    double dlon = lon2 - lon1;
    double dlat = lat2 - lat1;
    double a = Math.Pow(Math.Sin(dlat / 2), 2) +
               Math.Cos(lat1) * Math.Cos(lat2) *
               Math.Pow(Math.Sin(dlon / 2), 2);

    double c = 2 * Math.Asin(Math.Sqrt(a));

    // Radius of earth in
    // kilometers. Use 3956
    // for miles
    double r = 6371;

    // calculate the result
    return (c * r);
  }
  private double GetDistanceInKM(double Longitude1, double Longitude2, double Latitude1, double Latitude2)
  {

    double lat1 = Latitude1;
    double lat2 = Latitude2;
    double lon1 = Longitude1;
    double lon2 = Longitude2;

    double disttemp = Math.Round(distance(lat1, lat2, lon1, lon2), 1);

    return disttemp;
  }
  public Meter GetDistance(Point anotherPoint)
  {
    return new Meter(GetDistanceInKM(this.Location!.X, anotherPoint.X, this.Location.Y, anotherPoint.Y) * 1000);
  }
}