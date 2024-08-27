using System.Reflection;
using System.Text.Json;

namespace AEBackend.Repositories.Seeder;
public class PortData
{
  public string? name { get; set; }
  public string? city { get; set; }
  public string? country { get; set; }
  public List<object>? alias { get; set; }
  public List<object>? regions { get; set; }
  public List<double>? coordinates { get; set; }
  public string? province { get; set; }
  public string? timezone { get; set; }
  public List<string>? unlocs { get; set; }
  public string? code { get; set; }
}

public class PortsJsonLoader
{
  public static List<PortData>? LoadJson(ILogger logger)
  {
    logger.LogDebug("Reading ports.json....");
    using (StreamReader r = new StreamReader("Repositories/Seeder/ports.json"))
    {
      string json = r.ReadToEnd();
      Dictionary<string, PortData> items = JsonSerializer.Deserialize<Dictionary<string, PortData>>(json)!;

      List<PortData> result = [];
      Dictionary<string, bool> addedPortIds = new Dictionary<string, bool>();
      foreach (var item in items)
      {
        if (item.Value.unlocs != null && item.Value.coordinates != null)
        {
          string id = item.Value.unlocs[0];
          if (!addedPortIds.ContainsKey(id))
          {

            result.Add(item.Value);
            addedPortIds[id] = true;
          }
        }

      }
      return result;
    }
  }
}