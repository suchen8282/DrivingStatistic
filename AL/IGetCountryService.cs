using DrivingStatistic.BLL.Model;

namespace DrivingStatistic.AL
{
    public interface IGetCountryService
    {
        Task<string> GetCountryAsync(GPS gps);
    }
}
