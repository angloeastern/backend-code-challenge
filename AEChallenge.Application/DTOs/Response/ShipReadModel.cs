using Microsoft.EntityFrameworkCore;

[Keyless]
public class ShipReadModel
{
    public string ShipCode { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public int FiscalYear { get; set; }
    public string Status { get; set; } = string.Empty;


}