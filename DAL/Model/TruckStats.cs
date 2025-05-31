namespace DrivingStatistic.DAL.Model;
public class TruckStats
{
    public int Id { get; set; }
    public int DriverId { get; set; } // foreign key to Driver.Id
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime TimeStamp { get; set; } // index
    public int DriverAgeAtTimeStamp { get; set; } // index
    public string? Country  { get; set; } // index
}
