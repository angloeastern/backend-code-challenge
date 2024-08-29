namespace AEBackend.DomainModels;
public class NearestPortInfo
{
  public Port? Port { get; set; }
  public TimeSpan EstimatedArrivalTime { get; set; }
}