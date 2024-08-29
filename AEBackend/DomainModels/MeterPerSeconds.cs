using AEBackend.DomainModels;

public class MeterPerSeconds : Quantity
{
  public MeterPerSeconds(double value) : base("m/s", value)
  {
  }

  public TimeSpan GetSeconds(Meter distance)
  {
    return TimeSpan.FromSeconds(distance.Value / Value);
  }
}