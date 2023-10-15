using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IBookedFlightManager
    {
        Task<List<BookedFlight>> GetBookedFlightAsync();
        Task<bool> AddBookedFlightAsync(List<AddBookedFlight> addBookedFlights);
        Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int id);
    }
}
