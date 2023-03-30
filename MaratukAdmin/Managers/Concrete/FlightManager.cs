﻿using AutoMapper;
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

namespace MaratukAdmin.Managers.Concrete
{
    public class FlightManager : IFlightManager
    {
        private readonly IMainRepository<Flight> _mainRepository;
        private readonly IMapper _mapper;
        private readonly ICountryManager _countryManager;
        private readonly ICityManager _cityManager;
        private readonly IAircraftManager _aircraftManager;
        private readonly IAirlineManager _airlineManager;
        private readonly IAirportManager _airportManager;

        public FlightManager(IMainRepository<Flight> mainRepository,
                            IMapper mapper,
                            ICountryManager countryManager,
                            ICityManager cityManager,
                            IAircraftManager aircraftManager,
                            IAirlineManager airlineManager,
                            IAirportManager airportManager)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
            _countryManager = countryManager;
            _cityManager = cityManager;
            _aircraftManager = aircraftManager;
            _airlineManager = airlineManager;
            _airportManager = airportManager;
        }

        public async Task<Flight> AddFlightAsync(AddFlightRequest flight)
        {
            var flightDb = new Flight();

            // map the properties from AddFlightRequest to Flight
            flightDb.Name = flight.Name;
            flightDb.DepartureCountryId = flight.DepartureCountryId;
            flightDb.DepartureCityId = flight.DepartureCityId;
            flightDb.DepartureAirportId = flight.DepartureAirportId;
            flightDb.DestinationCountryId = flight.DestinationCountryId;
            flightDb.DestinationCityId = flight.DestinationCityId;
            flightDb.DestinationAirportId = flight.DestinationAirportId;
            flightDb.AirlineId = flight.AirlineId;
            flightDb.FlightValue = flight.FlightValue;
            flightDb.AircraftId = flight.AircraftId;

            // create a new list to hold the schedules
            var schedules = new List<Schedule>();

            // map the ScheduleRequests to Schedules
            foreach (var scheduleRequest in flight.Schedules)
            {
                var schedule = new Schedule();
                schedule.FlightStartDate = scheduleRequest.FlightStartDate;
                schedule.FlightEndDate = scheduleRequest.FlightEndDate;
                schedule.DepartureTime = scheduleRequest.DepartureTime;
                schedule.ArrivalTime = scheduleRequest.ArrivalTime;


                // convert int[] DayOfWeek to string DayOfWeek
                schedule.DayOfWeek = string.Join(",", scheduleRequest.DayOfWeek);

                schedules.Add(schedule);
            }

            // add the schedules to the flight
              flightDb.Schedules = schedules;



            return await _mainRepository.AddAsync(flightDb);
              
          
            
        }

        public async Task<Flight> UpdateFlightAsync(UpdateFlightRequest flight)
        {
            var entity = await _mainRepository.GetAsync(flight.Id, "Schedules");
            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }
            var entity1 = _mapper.Map<Flight>(flight);


            entity.DepartureAirportId = entity1.DepartureAirportId;
            entity.DepartureCityId = entity1.DepartureCityId;
            entity.DepartureAirportId = entity1.DepartureAirportId;
            entity.DestinationCountryId = entity1.DestinationCountryId;
            entity.DestinationCityId = entity1.DestinationCityId;
            entity.DestinationAirportId = entity1.DestinationAirportId;
            entity.AirlineId = entity1.AirlineId;
            entity.FlightValue = entity1.FlightValue;
            entity.AircraftId = entity1.AircraftId;

            entity.Schedules = entity1.Schedules;
            //todo add all params


            var result = await _mainRepository.UpdateAsync(entity);

            return result;
        }


        public async Task<bool> DeleteFlightAsync(int id)
        {
            var entity = await _mainRepository.GetAsync(id);
            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }


            await _mainRepository.DeleteAsync(id);


            return true;
        }

        public async Task<List<FlightResponse>> GetAllFlightAsync()
        {
            var result = await _mainRepository.GetAllAsync();


            var flightResponses = new List<FlightResponse>();


            foreach (var flight in result)
            {
                var flightRespons = new FlightResponse()
                {
                    Name = flight.Name,
                    Id = flight.Id,
                    DepartureCountry = _countryManager.GetCountryNameByIdAsync(flight.DepartureCountryId).Result.NameENG,
                    DestinationCountry = _countryManager.GetCountryNameByIdAsync(flight.DestinationCountryId).Result.NameENG,
                    FlightValue = flight.FlightValue,
                };
                flightResponses.Add(flightRespons);
            }
            return flightResponses;

        }

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

            flightEditResponse.scheduleInfos = sheduledEdit;

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

            flightInfoResponse.DepartureCountry = _countryManager.GetCountryNameByIdAsync(entity.DepartureCountryId).Result.NameENG;
            flightInfoResponse.DepartureCity = _cityManager.GetCityNameByIdAsync(entity.DepartureCityId).Result.NameEng;
            flightInfoResponse.DepartureAirport = _airportManager.GetAirportNameByIdAsync(entity.DepartureAirportId).Result.Name;


            flightInfoResponse.DestinationCountry = _countryManager.GetCountryNameByIdAsync(entity.DestinationCountryId).Result.NameENG;
            flightInfoResponse.DestinationCity = _cityManager.GetCityNameByIdAsync(entity.DestinationCityId).Result.NameEng;
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




        }
    }
}
