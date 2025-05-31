using AutoMapper;
using DrivingStatistic.AL;
using DrivingStatistic.DrivingStatistic.Interface.Model;
using GPS = DrivingStatistic.BLL.Model.GPS;

namespace DrivingStatistic.BLL
{
    public class DrivingStatisticProvider : IDrivingStatisticProvider
    {
        private readonly ISQLExecution _sqlExecution;
        private readonly IMapper _mapper;
        private readonly IGetCountryService _countryService;
        public DrivingStatisticProvider(ISQLExecution sqlExecution, IMapper mapper, IGetCountryService countryService)
        {
            _sqlExecution = sqlExecution;
            _mapper = mapper;
            _countryService = countryService;
        }

        public async Task<List<Driver>> GetDriversAsync()
        {
            var driverList = await _sqlExecution.GetDriversAsync();
            return _mapper.Map<List<Driver>>(driverList);
        }

        public async Task<Driver> InsertDriverAsync(DriverInput driverInput)
        {
            var dalDriver = _mapper.Map<DAL.Model.Driver>(driverInput);
            var insertedDriver = await _sqlExecution.AddDriverAsync(dalDriver);
            return _mapper.Map<Driver>(insertedDriver);
        }

        public async Task<List<TruckStats>> GetTruckPlansAsync()
        {
            var statsList = await _sqlExecution.GetAllTruckStatsAsync();
            return _mapper.Map<List<TruckStats>>(statsList);
        }

        public async Task<bool> InsertTruckPlanAsync(DriverCurrentLocation request)
        {
            if (await _sqlExecution.GetDriverAsync(request.DriverId) is null)
            {
                return false;
            }
            var stats = _mapper.Map<DAL.Model.TruckStats>(request.GPS);
            stats.DriverId = request.DriverId;
            return await _sqlExecution.AddTruckStatsAsync(stats);
        }

        public async Task<double?> GetDrivingDistanceAsync(int driverId)
        {
            if (await _sqlExecution.GetDriverAsync(driverId) is null)
            {
                throw new KeyNotFoundException($"Driver '{driverId}' not found.");
            }
            var statsList = await _sqlExecution.GetTruckStatsAsync(driverId);
            if (statsList is null || statsList.Count == 0)
            {
                return null;
            }

            var gps = _mapper.Map<List<GPS>>(statsList);
            return SumDistance(gps);
        }

        public async Task<double?> GetDrivingDistanceAsync(DistanceQuery query)
        {
            await ResolveCountryAsync();
            await ResolveDriverAgeAsync();
            if (query is null || query.Year < 2000 || query.Month < 1 || query.Month > 12)
            {
                throw new ArgumentException("Invalid query parameters.");
            }

            var queryDal = _mapper.Map<DAL.Model.DistanceQuery>(query);
            queryDal.PeriodStart = new DateTime(query.Year, query.Month, 1);
            queryDal.PeriodEnd = queryDal.PeriodStart.AddMonths(1).AddDays(-1);

            var statsList = await _sqlExecution.GetStatsByQueryAsync(queryDal);

            if (statsList is null || statsList.Count == 0)
            {
                return null;
            }

            List<List<DAL.Model.TruckStats>> statsDictionary = SplitByDriver(statsList);
            var gpsDictionary = _mapper.Map<List<List<GPS>>>(statsDictionary);

            double total = 0;
            for (int i = 0; i < gpsDictionary.Count; i++)
            {
                total += SumDistance(gpsDictionary[i]);
            }
            return total;
        }
        private static double SumDistance(List<GPS> gps)
        {
            double total = 0;
            for (int i = 1; i < gps.Count; i++)
            {
                total += HaversineDistanceCalculator.HaversineDistance(
                    gps[i - 1].Latitude, gps[i - 1].Longitude,
                    gps[i].Latitude, gps[i].Longitude
                );
            }
            return total;
        }

        private async Task ResolveCountryAsync()
        {
            var dalstats = await _sqlExecution.GetStatsWithoutCountryAsync();
            if (dalstats is null || dalstats.Count == 0)
            {
                return;
            }
            foreach (var dalstat in dalstats)
            {
                var gps = _mapper.Map<GPS>(dalstat);
                var country = await _countryService.GetCountryAsync(gps);
                
                if (country is not null)
                {
                    dalstat.Country = country;
                }
            }
            foreach (var stat in dalstats)
            {
                await _sqlExecution.UpdateStatsWithCountryAsync(stat);
            }
        }
        private async Task ResolveDriverAgeAsync()
        {
            var dalstats = await _sqlExecution.GetStatsWithoutAgeAsync();
            if (dalstats is null || dalstats.Count == 0)
            {
                return;
            }
            List<List<DAL.Model.TruckStats>> statsDictionary = SplitByDriver(dalstats);
            foreach (var singleDriverStats in statsDictionary)
            {
                var driver = await _sqlExecution.GetDriverAsync(singleDriverStats[0].DriverId);
                if (driver is null)
                {
                    continue;
                }
                singleDriverStats[0].DriverAgeAtTimeStamp =
                    AgeCalcualtor.CalculateAge(driver.Birthday, singleDriverStats[0].TimeStamp);
                foreach (var singleDriverstat in singleDriverStats)
                {
                    singleDriverstat.DriverAgeAtTimeStamp = singleDriverStats[0].DriverAgeAtTimeStamp;
                    await _sqlExecution.UpdateStatsWithAgeAsync(singleDriverstat);
                }
            }
        }

        private static List<List<DAL.Model.TruckStats>> SplitByDriver(List<DAL.Model.TruckStats> statsList)
        {
            return statsList
                .GroupBy(statsList => statsList.DriverId)
                .Select(group => group.ToList())
                .ToList();
        }
    }
}
