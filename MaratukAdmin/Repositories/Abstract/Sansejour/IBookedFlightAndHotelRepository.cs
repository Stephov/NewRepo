

using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IBookedFlightAndHotelRepository
    {
        Task<List<BookedInfoFlightPartResponse>> GetBookedInfoFlighPartAsync(BookedInfoFlightPartRequest request);
    }
}
