namespace AEBackend.DomainModels;
public class KmPerHour : Quantity
{
  public KmPerHour(double value) : base("Km/hour", value)
  {
  }

  public MeterPerSeconds ToMeterPerSeconds()
  {
    return new MeterPerSeconds(this.Value * 0.277778);
  }
}