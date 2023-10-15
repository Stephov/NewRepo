using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IBookedFlightRepository 
    {
        Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int agentId);
        Task<List<BookedFlight>> GetAllBookedFlightAsync();
        Task<BookedFlight> CreateBookedFlightAsync(BookedFlight bookedFlight);

    }
}
