using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IBookedFlightManager
    {
        Task<List<BookedFlightResponse>> GetBookedFlightAsync();
        Task<bool> AddBookedFlightAsync(List<AddBookedFlight> addBookedFlights);
        Task<List<BookedFlightResponse>> GetBookedFlightByAgentIdAsync(int id);
    }
}
