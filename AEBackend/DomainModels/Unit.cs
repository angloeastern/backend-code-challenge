namespace AEBackend.DomainModels;

public class Quantity
{
  public Quantity() { }

  public Quantity(string unitName, double value)
  {
    UnitName = unitName;
    Value = value;
  }

  public string UnitName { get; set; }
  public double Value { get; set; }

}