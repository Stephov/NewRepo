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
using System.Collections.Generic;

namespace MaratukAdmin.Managers.Concrete
{
    public class PriceBlockManager : IPriceBlockManager
    {
        private readonly IMainRepository<PriceBlock> _mainRepository;
        private readonly IPriceBlockRepository _priceBlockRepository;
        private readonly IMapper _mapper;
        private readonly ICountryManager _countryManager;
        private readonly ICityManager _cityManager;
        private readonly IFlightManager _flightManager;
        private readonly IPriceBlockTypeManager _priceBlockTypeManager;
        private readonly IPricePackageManager _pricePackageManager;
        private readonly IServiceClassManager _serviceClassManager;
        private readonly IAirlineManager _airlineManager;
        private readonly IAirportManager _airportManager;

        public PriceBlockManager(IMainRepository<PriceBlock> mainRepository,
                            IMapper mapper,
                            ICountryManager countryManager,
                            ICityManager cityManager,
                            IFlightManager flightManager,
                            IPriceBlockTypeManager priceBlockTypeManager,
                            IPricePackageManager pricePackageManager,
                            IServiceClassManager serviceClassManage,
                            IAirlineManager airlineManager,
                            IAirportManager airportManager,
                            IPriceBlockRepository priceBlockRepository
                            )
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
            _countryManager = countryManager;
            _cityManager = cityManager;
            _flightManager = flightManager;
            _pricePackageManager = pricePackageManager;
            _serviceClassManager = serviceClassManage;
            _airlineManager = airlineManager;
            _airportManager = airportManager;
            _priceBlockTypeManager = priceBlockTypeManager;
            _priceBlockRepository = priceBlockRepository;
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



            return await _mainRepository.AddAsync(priceBlockDb);



        }

        public async Task<PriceBlock> UpdatePriceBlockAsync(UpdatePriceBlockRequest price)
        {
            var entity = await _mainRepository.GetAsync(price.Id);
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

            // map the ScheduleRequests to Schedules


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
            try
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
            catch (Exception ex)
            {
                var mes = ex.Message;
            }
            return null;




        }


        public async Task<PriceBlockEditResponse> GetPriceBlockByIdAsync(int id)
        {
            var entity = await _mainRepository.GetAsync(id);
            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            var priceBlockEditResponse = new PriceBlockEditResponse();
            var priceBlockServiceEdit = new List<PriceBlockServiceEditResponse>();

            priceBlockEditResponse.Name = entity.Name;
            priceBlockEditResponse.Id = entity.Id;

            priceBlockEditResponse.PricelBlockTypeId = entity.PricelBlockTypeId;
            priceBlockEditResponse.PricePackageId = entity.PricePackageId;
            priceBlockEditResponse.ServiceClassId = entity.ServiceClassId;


            priceBlockEditResponse.SeasonId = entity.SeasonId;
            priceBlockEditResponse.PartnerId = entity.PartnerId;
            priceBlockEditResponse.CurrencyId = entity.CurrencyId;

            priceBlockEditResponse.Comments = entity.Comments;
            priceBlockEditResponse.TarifId = entity.TarifId;


            return priceBlockEditResponse;

        }

        public async Task<PriceBlockServices> AddPriceBlockServicesAsync(AddPriceBlockServicesRequest priceBlockService)
        {
            var priceBlockServicesDb = new PriceBlockServices();

            // map the properties from AddFlightRequest to Flight
            priceBlockServicesDb.DepartureCountryId = priceBlockService.DepartureCountryId;
            priceBlockServicesDb.DepartureCityId = priceBlockService.DepartureCityId;
            priceBlockServicesDb.DestinationCountryId = priceBlockService.DestinationCountryId;
            priceBlockServicesDb.DestinationCityId = priceBlockService.DestinationCityId;
            priceBlockServicesDb.FlightId = priceBlockService.FligthId;
            priceBlockServicesDb.PriceBlockId = priceBlockService.PriceBlockId;




            return await _priceBlockRepository.CreatePriceBlockServicesAsync(priceBlockServicesDb);
        }

        public async Task<bool> DeletePriceBlockServiceAsync(int id)
        {
            return await _priceBlockRepository.DeletePriceBlockServicesAsync(id);
        }

        public async Task<List<PriceBlockServicesResponse>> GetServicesByPriceBlockIdAsync(int id)
        {
            var res = await _priceBlockRepository.GetServicesByPriceBlockIdAsync(id);

            List<PriceBlockServicesResponse> data = new List<PriceBlockServicesResponse>();

            foreach (var service in res)
            {

                try
                {
                    PriceBlockServicesResponse priceBlockServicesResponse = new PriceBlockServicesResponse();
                    priceBlockServicesResponse.PriceBlockServicesId = service.Id;
                    priceBlockServicesResponse.DepartureCountryName = _countryManager.GetCountryNameByIdAsync(service.DepartureCountryId).Result.NameENG;
                    priceBlockServicesResponse.DepartureCityName = _cityManager.GetCityNameByIdAsync(service.DepartureCityId).Result.Name;
                    priceBlockServicesResponse.DestinationCountryName = _countryManager.GetCountryNameByIdAsync(service.DestinationCountryId).Result.NameENG;
                    priceBlockServicesResponse.DestinationCityName = _cityManager.GetCityNameByIdAsync(service.DestinationCityId).Result.Name;
                    priceBlockServicesResponse.FlightName = _flightManager.GetFlightInfoByIdAsync(service.FlightId).Result.Name;
                    priceBlockServicesResponse.FlightValue = _flightManager.GetFlightInfoByIdAsync(service.FlightId).Result.FlightValue;
                    data.Add(priceBlockServicesResponse);

                }
                catch (Exception ex)
                {
                    return data;
                }


            }

            return data;
        }

        public async Task<ServicesPricingPolicy> CreateServicesPricingPolicyAsync(AddServicesPricingPolicy addServicesPricingPolicy)
        {
            ServicesPricingPolicy servicesPricingPolicyDB = new ServicesPricingPolicy();

            servicesPricingPolicyDB.PriceBlockServicesId = addServicesPricingPolicy.PriceBlockServicesId;
            servicesPricingPolicyDB.CurrencyId = addServicesPricingPolicy.CurrencyId;
            servicesPricingPolicyDB.Netto = addServicesPricingPolicy.Netto;
            servicesPricingPolicyDB.Parcent = addServicesPricingPolicy.Parcent;
            servicesPricingPolicyDB.Bruto = addServicesPricingPolicy.Bruto;
            servicesPricingPolicyDB.StartDate = addServicesPricingPolicy.StartDate;
            servicesPricingPolicyDB.EndDate = addServicesPricingPolicy.EndDate;
            servicesPricingPolicyDB.SaleDate = addServicesPricingPolicy.SaleDate;
            servicesPricingPolicyDB.AgeFrom = addServicesPricingPolicy.AgeFrom;
            servicesPricingPolicyDB.AgeUpTo = addServicesPricingPolicy.AgeUpTo;
            servicesPricingPolicyDB.CountFrom = addServicesPricingPolicy.CountFrom;
            servicesPricingPolicyDB.CountUpTo = addServicesPricingPolicy.CountUpTo;



            return await _priceBlockRepository.CreateServicesPricingPolicyAsync(servicesPricingPolicyDB);


        }

        public async Task<ServicesPricingPolicy> UpdateServicesPricingPolicyAsync(EditServicesPricingPolicy editServicesPricingPolicy)
        {
            ServicesPricingPolicy res = await _priceBlockRepository.GetServicesPricingPolicyByIdAsync(editServicesPricingPolicy.Id);




            res.CurrencyId = editServicesPricingPolicy.CurrencyId;
            res.Netto = editServicesPricingPolicy.Netto;
            res.Parcent = editServicesPricingPolicy.Parcent;
            res.Bruto = editServicesPricingPolicy.Bruto;
            res.StartDate = editServicesPricingPolicy.StartDate;
            res.EndDate = editServicesPricingPolicy.EndDate;
            res.SaleDate = editServicesPricingPolicy.SaleDate;
            res.AgeFrom = editServicesPricingPolicy.AgeFrom;
            res.AgeUpTo = editServicesPricingPolicy.AgeUpTo;
            res.CountFrom = editServicesPricingPolicy.CountFrom;
            res.CountUpTo = editServicesPricingPolicy.CountUpTo;



            var result = await _priceBlockRepository.UpdateServicesPricingPolicyAsync(res);

            return result;


        }



        public async Task<bool> DeleteServicesPricingPolicyAsync(int id)
        {
            return await _priceBlockRepository.DeleteServicesPricingPolicyAsync(id);
        }

        public async Task<List<ServicesPricingPolicy>> GetServicesPricingPolicyByPriceBlockServicesIdAsync(int id)
        {
            return await _priceBlockRepository.GetServicesPricingPolicyByPriceBlockServicesIdAsync(id);
        }
        /*
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
