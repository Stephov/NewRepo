using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IBookedHotelManager
    {
        //Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn);
        Task<bool> AddBookedHotelAsync(AddBookHotelRequest addBookedHotel);
        //Task<BookedFlightResponseFinal> GetBookedFlightByAgentIdAsync(int id);
    }
}
