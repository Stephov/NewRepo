using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.AutoMapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<AddFlightRequest, Flight>()
              .ForMember(dto => dto.Schedules, opt => opt.MapFrom(x => x.Schedules));

            CreateMap<UpdateFlightRequest, Flight>()
              .ForMember(dto => dto.Schedules, opt => opt.MapFrom(x => x.Schedules));





            CreateMap<ScheduleRequest, Schedule>()
                .ForMember(dto => dto.DayOfWeek, opt => opt.MapFrom(x => String.Join(",", x)));


            CreateMap<AddPricePackage, PricePackage>();
            CreateMap<UpdatePricePackage, PricePackage>();
            CreateMap<AddAirline, Airline>();
            CreateMap<AddAircraft, Aircraft>();
            CreateMap<AddAirport, Airport>();


        }
    }
}
