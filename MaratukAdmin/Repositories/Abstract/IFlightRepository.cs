using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync();
        Task<Flight> GetFlightByIdAsync(int id);
        Task<Flight> CreateFlightAsync(Flight flight);
        Task UpdateFlightAsync(Flight flight);
        Task DeleteFlightAsync(int id);

        Task<IEnumerable<Schedule>> GetSchedulesByFlightIdAsync(int flightId);
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(int id);
    }
}
