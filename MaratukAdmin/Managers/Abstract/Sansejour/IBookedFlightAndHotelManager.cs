using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IBookedFlightAndHotelManager
    {
        //Task<string> AddBookedFlightAndHotelAsync(List<AddBookedFlight> addBookedFlights);
        Task<string> AddBookedFlightAndHotelAsync(BookedFlightAndHotel bookedFlightAndHotel);
        Task<List<BookedHotelResponse>> GetBookedFlightsAsync(int Itn);
        Task<List<BookedInfoFlightPartResponse>> GetBookedInfoFlighPartAsync(BookedInfoFlightPartRequest request);
        Task<List<BookedInfoFlightPartGroupedResponse>> GetBookedInfoFlighPartGroupAsync(List<BookedInfoFlightPartResponse> request);


        //Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn);
        //Task<BookedFlightResponseFinal> GetBookedFlightByAgentIdAsync(int id);
    }
}
