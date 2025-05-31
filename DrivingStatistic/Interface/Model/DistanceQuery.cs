namespace DrivingStatistic.DrivingStatistic.Interface.Model;
public class DistanceQuery
{
    public int Age { get; set; }
    public required string Country { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}