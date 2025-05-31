namespace DrivingStatistic.BLL;
using DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISQLExecution
{
    Task<List<Driver>> GetDriversAsync();
    Task<Driver?> GetDriverAsync(int id);
    Task<Driver> AddDriverAsync(Driver driver);
    Task<List<TruckStats>> GetAllTruckStatsAsync();
    Task<List<TruckStats>> GetTruckStatsAsync(int driverId);
    Task<bool> AddTruckStatsAsync(TruckStats TruckStats);
    Task<List<TruckStats>> GetStatsWithoutCountryAsync();
    Task<bool> UpdateStatsWithCountryAsync(TruckStats TruckStats);
    Task<List<TruckStats>> GetStatsWithoutAgeAsync();
    Task<bool> UpdateStatsWithAgeAsync(TruckStats TruckStats);
    Task<List<TruckStats>> GetStatsByQueryAsync(DistanceQuery query);

}
