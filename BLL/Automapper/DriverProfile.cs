using AutoMapper;

namespace DrivingStatistic.BLL.Automapper
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Model.GPS, DAL.Model.TruckStats>();
        }
    }
}