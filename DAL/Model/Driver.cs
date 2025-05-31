namespace DrivingStatistic.DAL.Model;
public class Driver
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime Birthday { get; set; }
}