namespace DrivingStatistic.DrivingStatistic.Interface.Model;
public class TruckStats
{
    public int DriverId { get; set; }
    public required GPS GPS { get; set; }
    public int DriverAgeAtTimeStamp { get; set; }
    public string? Country { get; set; }
}
