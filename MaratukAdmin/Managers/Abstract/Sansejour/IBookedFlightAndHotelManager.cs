using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IBookedFlightAndHotelManager
    {
        //Task<string> AddBookedFlightAndHotelAsync(List<AddBookedFlight> addBookedFlights);
        Task<string> AddBookedFlightAndHotelAsync(BookedFlightAndHotel bookedFlightAndHotel);
        Task<List<BookedHotelResponse>> GetBookedHotelAsync(int Itn);

        //Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn);
        //Task<BookedFlightResponseFinal> GetBookedFlightByAgentIdAsync(int id);
    }
}
