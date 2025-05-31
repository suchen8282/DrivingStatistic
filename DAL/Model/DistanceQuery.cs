namespace DrivingStatistic.DAL.Model;
public class DistanceQuery
{
    public int Age { get; set; }
    public required string Country { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}