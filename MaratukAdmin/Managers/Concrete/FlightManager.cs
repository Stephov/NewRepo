using AutoMapper;
using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;

namespace MaratukAdmin.Managers.Concrete
{
    public class FlightManager : IFlightManager
    {
        private readonly IMainRepository<Flight> _mainRepository;
        private readonly IMapper _mapper;
        public FlightManager(IMainRepository<Flight> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<Flight> AddFlightAsync(AddFlightRequest flight)
        {
            var entity1 = _mapper.Map<Flight>(flight);
            return await _mainRepository.AddAsync(entity1);
        }

        public async Task<Flight> UpdateFlightAsync(UpdateFlightRequest flight)
        {
            var entity = await _mainRepository.GetAsync(flight.Id, "Schedules");
            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }
            var entity1 = _mapper.Map<Flight>(flight);


            entity.DepartureAirportId= entity1.DepartureAirportId;
            entity.DepartureCityId= entity1.DepartureCityId;
            entity.DepartureAirportId = entity1.DepartureAirportId;
            entity.DestinationCountryId= entity1.DestinationCountryId;
            entity.DestinationCityId = entity1.DestinationCityId;
            entity.DestinationAirportId = entity1.DestinationAirportId;
            entity.AirlineId= entity1.AirlineId;
            entity.FlightValue = entity1.FlightValue;
            entity.AircraftId= entity1.AircraftId;

            entity.Schedules = entity1.Schedules;
            //todo add all params
            

            var result = await _mainRepository.UpdateAsync(entity);

            return result;
        }


        public async Task<bool> DeleteFlightAsync( int id)
        {
            var entity = await _mainRepository.GetAsync(id);
            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }


            await _mainRepository.DeleteAsync(id);


            return true;
        }

        public async  Task<List<Flight>> GetAllFlightAsync()
        {
           return await _mainRepository.GetAllAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }
    }
}
