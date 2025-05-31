using Microsoft.AspNetCore.Mvc;
using DrivingStatistic.BLL;
using DrivingStatistic.DrivingStatistic.Interface.Model;

namespace DrivingStatistic.SL.Controller
{
    [ApiController]
    //[Route("[controller]")]
    public class DrivingStatisticController : ControllerBase
    {
        private readonly IDrivingStatisticProvider _truckPlanProvider;
        public DrivingStatisticController(IDrivingStatisticProvider truckPlanProvider)
        {
            _truckPlanProvider = truckPlanProvider;
        }

        [HttpGet("driver")]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _truckPlanProvider.GetDriversAsync();
        }

        [HttpPost("driver")]
        public async Task<ActionResult<Driver>> CreateDriver([FromBody] DriverInput driver)
        {
            return await _truckPlanProvider.InsertDriverAsync(driver);
        }

        [HttpGet("TruckPlan")]
        public async Task<ActionResult<IEnumerable<TruckStats>>> GetTruckPlans()
        {
            return await _truckPlanProvider.GetTruckPlansAsync();
        }

        [HttpPost("TruckPlan")]
        public async Task<ActionResult<bool>> CreateTruckPlan([FromBody] DriverCurrentLocation request)
        {
            var result = await _truckPlanProvider.InsertTruckPlanAsync(request);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"Driver '{request.DriverId}' not found or log could not be created.");
            }
        }

        [HttpGet("DrivingDistance/by-driver")]
        public async Task<ActionResult<double>> GetDrivingDistance(int driverId)
        {
            double? result;
            try
            {
                result = await _truckPlanProvider.GetDrivingDistanceAsync(driverId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            if (result.HasValue)
            {
                return Ok(result.Value);
            }
            else
            {
                return NotFound($"No driving distance found for driver '{driverId}'.");
            }
        }

        [HttpGet("DrivingDistance/by-query")]
        public async Task<ActionResult<double>> GetDrivingDistance(
            [FromQuery]  DistanceQuery query)
        {
            return await _truckPlanProvider.GetDrivingDistanceAsync(query);
        }

    }
}