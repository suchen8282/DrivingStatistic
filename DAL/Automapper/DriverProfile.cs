using AutoMapper;


namespace DrivingStatistic.DAL.Automapper
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Model.Driver, DrivingStatistic.Interface.Model.Driver>()
                .ForPath(dest => dest.DriverInput.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.DriverInput.Birthday, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Birthday)));

            CreateMap<Model.TruckStats, DrivingStatistic.Interface.Model.TruckStats>()
                .ForPath(dest => dest.GPS.Latitude, opt => opt.MapFrom(src =>src.Latitude))
                .ForPath(dest => dest.GPS.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForPath(dest => dest.GPS.Timestamp, opt => opt.MapFrom(src => src.TimeStamp));

            CreateMap<Model.TruckStats, BLL.Model.GPS>();
        }

    }
}