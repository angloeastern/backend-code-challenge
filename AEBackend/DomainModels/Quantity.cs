namespace AEBackend.DomainModels;

public class Quantity
{
  public Quantity() { }

  public Quantity(string unitName, double value)
  {
    UnitName = unitName;
    Value = value;
  }

  public string UnitName { get; set; } = string.Empty;
  public double Value { get; set; } = 0;

}