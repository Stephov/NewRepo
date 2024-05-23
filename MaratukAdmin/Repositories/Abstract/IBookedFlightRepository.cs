using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IBookedFlightRepository 
    {
        Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int agentId);
        Task<List<BookedFlight>> GetAllBookedFlightAsync(List<AgencyUser> agencyUsers);
        Task<BookedFlight> CreateBookedFlightAsync(BookedFlight bookedFlight);
        Task<List<BookedFlight>> GetBookedFlightByMaratukAgentIdAsync(int maratukAgent);
        Task<List<BookedFlight>> GetBookedFlightForHotelManagerAsync(int maratukAgent);
        Task<BookedFlight> GetBookedFlightByIdAsync(int Id);
        Task<List<BookedFlight>> GetBookedFlightByOrderNumberAsync(string orderNumber);
        Task<List<BookedFlight>> GetBookedFlightByMaratukAgentForAccAsync();
        Task<int> GetBookedFlightCountAsync();
        Task UpdateBookedFlightsAsync(List<BookedFlight> bookedFlights);
        Task<ReturnStatusResponse> SetTicketNumberToBookAsync(SetTicketNumberToBookRequest request);
    }
}
