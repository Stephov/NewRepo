using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IBookedFlightManager
    {
        Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn);
        Task<bool> AddBookedFlightAsync(List<AddBookedFlight> addBookedFlights);
        Task<BookedFlightResponseFinal> GetBookedFlightByAgentIdAsync(int id);
    }
}
