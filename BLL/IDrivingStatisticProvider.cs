using DrivingStatistic.DrivingStatistic.Interface.Model;

namespace DrivingStatistic.BLL
{
    public interface IDrivingStatisticProvider
    {
        Task<Driver> InsertDriverAsync(DriverInput driver);
        Task<List<Driver>> GetDriversAsync();
        Task<bool> InsertTruckPlanAsync(DriverCurrentLocation request);
        Task<List<TruckStats>> GetTruckPlansAsync();
        Task<double?> GetDrivingDistanceAsync(int driverId);
        Task<double?> GetDrivingDistanceAsync(DistanceQuery query);
    }
}
