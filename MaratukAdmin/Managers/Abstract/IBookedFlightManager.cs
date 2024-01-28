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
        Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightByMaratukAgentIdAsync(int maratukAgent, int pageNumber = 1, int pageSize = 10);
        Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightForAccAsync(int pageNumber, int pageSize);
        Task<bool> UpdateBookedStatusAsync(string orderNumber, int status);
        Task<BookedFlight> UpdateBookedUserInfoAsync(BookedUserInfoForMaratukRequest bookedUserInfoForMaratuk);
    }
}
