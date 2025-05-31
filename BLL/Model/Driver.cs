namespace DrivingStatistic.BLL.Model;
public class Driver
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateOnly Birthday { get; set; }
}