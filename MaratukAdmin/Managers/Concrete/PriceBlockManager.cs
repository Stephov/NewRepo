using AutoMapper;
using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Utils;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MaratukAdmin.Managers.Concrete
{
    public class PriceBlockManager : IPriceBlockManager
    {
        private readonly IMainRepository<PriceBlock> _mainRepository;
        private readonly IMapper _mapper;
        private readonly ICountryManager _countryManager;
        private readonly IPriceBlockTypeManager _priceBlockTypeManager;
        private readonly IPricePackageManager _pricePackageManager;
        private readonly IServiceClassManager _serviceClassManager;
        private readonly IAirlineManager _airlineManager;
        private readonly IAirportManager _airportManager;

        public PriceBlockManager(IMainRepository<PriceBlock> mainRepository,
                            IMapper mapper,
                            ICountryManager countryManager,
                            IPriceBlockTypeManager priceBlockTypeManager,
                            IPricePackageManager pricePackageManager,
                            IServiceClassManager serviceClassManage,
                            IAirlineManager airlineManager,
                            IAirportManager airportManager)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
            _countryManager = countryManager;
            _pricePackageManager = pricePackageManager;
            _serviceClassManager = serviceClassManage;
            _airlineManager = airlineManager;
            _airportManager = airportManager;
            _priceBlockTypeManager = priceBlockTypeManager;
        }

        public async Task<PriceBlock> AddPriceBlockAsync(AddPriceBlockRequest priceBlockRequest)
        {
            var priceBlockDb = new PriceBlock();

            // map the properties from AddFlightRequest to Flight
            priceBlockDb.Name = priceBlockRequest.Name;
            priceBlockDb.PricelBlockTypeId = priceBlockRequest.PricelBlockTypeId;
            priceBlockDb.PricePackageId = priceBlockRequest.PricePackageId;
            priceBlockDb.ServiceClassId = priceBlockRequest.ServiceClassId;
            priceBlockDb.SeasonId = priceBlockRequest.SeasonId;
            priceBlockDb.PartnerId = priceBlockRequest.PartnerId;
            priceBlockDb.CurrencyId = priceBlockRequest.CurrencyId;
            priceBlockDb.Comments = priceBlockRequest.Comments;
            priceBlockDb.TarifId = priceBlockRequest.TarifId;


            // create a new list to hold the schedules
            var priceBlockServices = new List<PriceBlockServices>();

            // map the ScheduleRequests to Schedules
            foreach (var service in priceBlockRequest.PriceBlockService)
            {
                var priceBlockService = new PriceBlockServices();

                priceBlockService.DepartureCountryId = service.DepartureCountryId;
                priceBlockService.DepartureCityId = service.DepartureCityId;
                priceBlockService.DestinationCountryId = service.DestinationCountryId;
                priceBlockService.DestinationCityId = service.DestinationCityId;

                priceBlockService.CurrencyId = service.CurrencyId;
                priceBlockService.Netto = service.Netto;
                priceBlockService.Parcent = service.Parcent;
                priceBlockService.Bruto = service.Bruto;
                priceBlockService.StartDate = service.StartDate;
                priceBlockService.EndDate = service.EndDate;
                priceBlockService.SaleDate = service.SaleDate;
                priceBlockService.AgeFrom = service.AgeFrom;
                priceBlockService.AgeUpTo = service.AgeUpTo;
                priceBlockService.CountFrom = service.CountFrom;
                priceBlockService.CountUpTo = service.CountUpTo;



                priceBlockServices.Add(priceBlockService);
            }

            // add the schedules to the flight
            priceBlockDb.Services = priceBlockServices;



            return await _mainRepository.AddAsync(priceBlockDb);



        }

        public async Task<PriceBlock> UpdatePriceBlockAsync(UpdatePriceBlockRequest price)
        {
            var entity = await _mainRepository.GetAsync(price.Id, "Services");
            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }


            entity.PricelBlockTypeId = price.PricelBlockTypeId;
            entity.PricePackageId = price.PricePackageId;
            entity.ServiceClassId = price.ServiceClassId;
            entity.SeasonId = price.SeasonId;
            entity.PartnerId = price.PartnerId;
            entity.CurrencyId = price.CurrencyId;
            entity.Name = price.Name;
            entity.Comments = price.Comments;
            entity.TarifId = price.TarifId;

            // create a new list to hold the schedules
            var priceBlockServices = new List<PriceBlockServices>();

            // map the ScheduleRequests to Schedules
            foreach (var service in price.PriceBlockService)
            {
                var priceBlockService = new PriceBlockServices();

                priceBlockService.DepartureCountryId = service.DepartureCountryId;
                priceBlockService.DepartureCityId = service.DepartureCityId;
                priceBlockService.DestinationCountryId = service.DestinationCountryId;
                priceBlockService.DestinationCityId = service.DestinationCityId;

                priceBlockService.CurrencyId = service.CurrencyId;
                priceBlockService.Netto = service.Netto;
                priceBlockService.Parcent = service.Parcent;
                priceBlockService.Bruto = service.Bruto;
                priceBlockService.StartDate = service.StartDate;
                priceBlockService.EndDate = service.EndDate;
                priceBlockService.SaleDate = service.SaleDate;
                priceBlockService.AgeFrom = service.AgeFrom;
                priceBlockService.AgeUpTo = service.AgeUpTo;
                priceBlockService.CountFrom = service.CountFrom;
                priceBlockService.CountUpTo = service.CountUpTo;

                priceBlockServices.Add(priceBlockService);
            }

            // add the schedules to the flight
            entity.Services = priceBlockServices;

            var result = await _mainRepository.UpdateAsync(entity);

            return result;
        }


        public async Task<bool> DeletePriceBlockAsync(int id)
        {
            var entity = await _mainRepository.GetAsync(id);
            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }


            await _mainRepository.DeleteAsync(id);


            return true;
        }

        public async Task<List<PriceBlockResponse>> GetAllPriceBlockAsync()
        {
            var result = await _mainRepository.GetAllAsync();


            var priceBlockResponses = new List<PriceBlockResponse>();


            foreach (var priceBlock in result)
            {
                var priceBlockRespons = new PriceBlockResponse()
                {
                    Name = priceBlock.Name,
                    Id = priceBlock.Id,
                    PriceBlockType = _priceBlockTypeManager.GetPriceBlockTypeNameByIdAsync(priceBlock.PricelBlockTypeId).Result.Name,
                    PricePackageId = _pricePackageManager.GetPricePackageByIdAsync(priceBlock.PricePackageId).Result.Name,
                    ServiceClassId = _serviceClassManager.GetServiceClassNameByIdAsync(priceBlock.ServiceClassId).Result.Name,

                };
                priceBlockResponses.Add(priceBlockRespons);
            }
            return priceBlockResponses;

        }

        /* 
                       public async Task<FlightEditResponse> GetFlightByIdAsync(int id)
                       {
                           var entity = await _mainRepository.GetAsync(id, "Schedules");
                           if (entity == null)
                           {
                               throw new ApiBaseException(StatusCodes.Status404NotFound);
                           }

                           var flightEditResponse = new FlightEditResponse();
                           var sheduledEdit = new List<ScheduleEditResponse>();

                           flightEditResponse.Name = entity.Name;
                           flightEditResponse.Id = entity.Id;

                           flightEditResponse.DepartureCountryId = entity.DepartureCountryId;
                           flightEditResponse.DepartureCityId = entity.DepartureCityId;
                           flightEditResponse.DepartureAirportId = entity.DepartureAirportId;


                           flightEditResponse.DestinationCountryId = entity.DestinationCountryId;
                           flightEditResponse.DestinationCityId = entity.DestinationCityId;
                           flightEditResponse.DestinationAirportId = entity.DestinationAirportId;

                           flightEditResponse.FlightValue = entity.FlightValue;
                           flightEditResponse.AirlineId = entity.AirlineId;
                           flightEditResponse.AircraftId = entity.AircraftId;


                           if (entity.Schedules != null)
                           {
                               foreach (var shedul in entity.Schedules)
                               {
                                   var schedule = new ScheduleEditResponse()
                                   {
                                       FlightStartDate = shedul.FlightStartDate,
                                       FlightEndDate = shedul.FlightEndDate,
                                       DepartureTime = shedul.DepartureTime,
                                       ArrivalTime = shedul.ArrivalTime,
                                       DayOfWeek = shedul.DayOfWeek.Split(',').Select(int.Parse).ToArray()
                               };



                                   sheduledEdit.Add(schedule);

                               }
                           }

                           flightEditResponse.schedules = sheduledEdit;

                           return flightEditResponse;

                       }

                       public async Task<FlightInfoResponse> GetFlightInfoByIdAsync(int id)
                       {
                           var entity = await _mainRepository.GetAsync(id, "Schedules");

                           if (entity == null)
                           {
                               throw new ApiBaseException(StatusCodes.Status404NotFound);
                           }



                           var flightInfoResponse = new FlightInfoResponse();
                           var sheduledInfo = new List<ScheduleInfoResponse>();


                           flightInfoResponse.Name = entity.Name;
                           flightInfoResponse.Id = entity.Id;

                           flightInfoResponse.DepartureCountry = _countryManager.GetCountryNameByIdAsync(entity.DepartureCountryId).Result.Name;
                           flightInfoResponse.DepartureCity = _cityManager.GetCityNameByIdAsync(entity.DepartureCityId).Result.Name;
                           flightInfoResponse.DepartureAirport = _airportManager.GetAirportNameByIdAsync(entity.DepartureAirportId).Result.Name;


                           flightInfoResponse.DestinationCountry = _countryManager.GetCountryNameByIdAsync(entity.DestinationCountryId).Result.Name;
                           flightInfoResponse.DestinationCity = _cityManager.GetCityNameByIdAsync(entity.DestinationCityId).Result.Name;
                           flightInfoResponse.DestinationAirport = _airportManager.GetAirportNameByIdAsync(entity.DestinationAirportId).Result.Name;

                           flightInfoResponse.FlightValue = entity.FlightValue;
                           flightInfoResponse.Airline = _airlineManager.GetAirlineNameByIdAsync(entity.AirlineId).Result.Name;
                           flightInfoResponse.Aircraft = _aircraftManager.GetAircraftNameByIdAsync(entity.AircraftId).Result.Name;

                          if (entity.Schedules != null)
                           {
                               foreach (var shedul in entity.Schedules)
                               {
                                   var schedule = new ScheduleInfoResponse()
                                   {
                                       FlightStartDate = shedul.FlightStartDate,
                                       FlightEndDate = shedul.FlightEndDate,
                                       DepartureTime = shedul.DepartureTime,
                                       ArrivalTime = shedul.ArrivalTime,
                                       DayOfWeek = HandleWeekDays.GetWeekDayNames(shedul.DayOfWeek),
                                   };



                                   sheduledInfo.Add(schedule);

                               }
                           }

                           flightInfoResponse.scheduleInfos = sheduledInfo;





                           return flightInfoResponse;




                       }*/
    }
}
