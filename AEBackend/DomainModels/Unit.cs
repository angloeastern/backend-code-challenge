namespace AEBackend.DomainModels;

public class Quantity
{
  private string _unitName;
  private double _value;

  public Quantity(string unitName, double value)
  {
    _unitName = unitName;
    _value = value;
  }

  public string UnitName
  {
    get
    {
      return _unitName;
    }
  }
  public double Value
  {
    get
    {
      return _value;
    }
  }

}