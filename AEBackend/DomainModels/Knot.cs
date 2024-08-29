
namespace AEBackend.DomainModels;
public class Knot : Quantity
{
  public Knot() : base("Knot", 0)
  {

  }
  public Knot(double value) : base("Knot", value)
  {

  }

  internal static Knot Zero()
  {
    return new Knot(0);
  }

  public KmPerHour ToKMPerHour()
  {
    return new KmPerHour(this.Value * 1.852);
  }

  public TimeSpan ArrivalTimeTo(Meter distance)
  {
    var meterPerSeconds = ToKMPerHour().ToMeterPerSeconds();
    TimeSpan seconds = meterPerSeconds.GetSeconds(distance);

    return seconds;
  }
}