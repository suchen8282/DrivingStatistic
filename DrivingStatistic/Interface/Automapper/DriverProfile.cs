using AutoMapper;
using DrivingStatistic.DrivingStatistic.Interface.Model;


namespace DrivingStatistic.Interface.Automapper
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<DriverInput, DAL.Model.Driver>()
              .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday.ToDateTime(TimeOnly.MinValue))); ;
            CreateMap<GPS, DAL.Model.TruckStats>();
            CreateMap<DistanceQuery, DAL.Model.DistanceQuery>();
        }

    }
}