using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Models;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync();
        Task<List<FlightCountry>> GetAllCountryFlightsAsync();
        Task<Flight> GetFlightByIdAsync(int id);

        Task<List<Flight>> GetFlightByIdsAsync(int departureCountryId,int departureCityId,int DestinationCountryId, int destinationCityId);
        Task<Flight> CreateFlightAsync(Flight flight);
        Task UpdateFlightAsync(Flight flight);
        Task DeleteFlightAsync(int id);

        Task<IEnumerable<Schedule>> GetSchedulesByFlightIdAsync(int flightId);
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(int id);
        Task<bool> IsisFlightNameExistsAsync(string flightName);


    }
}
