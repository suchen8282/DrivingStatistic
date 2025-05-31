namespace DrivingStatistic.DrivingStatistic.Interface.Model;
public class DriverCurrentLocation
{
    public int DriverId { get; set; }
    public required GPS GPS { get; set; }
}