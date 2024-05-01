

using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IBookedFlightAndHotelRepository
    {
        Task<List<BookedInfoFlightPartResponse>> GetBookedInfoFlighPartAsync(BookedInfoFlightPartRequest request);
        Task<string> PayForBookedFlightAndHotelAsync(PayForBookedFlightAndHotelRequest payForBookedFlightAndHotel);
        Task<BookPayment> GetBookPaymentAsync(int? id, string? orderNumber, string? paymentNumber);
        Task<List<BookPayment>> GetBookPaymentsByOrderNumberAndPaymentStatusAsync(string orderNumber, int? paymentStatus = null);

        Task UpdateBookPaymentAsync(BookPayment bookPayment);
    }
}
