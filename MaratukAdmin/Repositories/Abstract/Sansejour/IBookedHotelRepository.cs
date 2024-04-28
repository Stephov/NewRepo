using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using System.Threading.Tasks;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IBookedHotelRepository
    {
        //Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int agentId);
        Task<List<BookedHotelResponse>> GetAllBookedHotelsAsync(List<AgencyUser> agencyUsers);

        Task<BookedHotel> GetAllBookedHotelsAsync(string orderID);
        //Task<BookedHotel> CreateBookedHotelAsync(BookedHotel bookedHotel, List<BookedHotelGuest> bookedHotelGuests);
        Task<BookedHotel> CreateBookedHotelAsync(BookedHotel bookedHotel);
        Task<BookInvoiceData> AddBookedHotelInvoiceDataAsync(BookInvoiceData invoiceData);
        Task<BookInvoiceData>? GetBookInvoiceDataAsync(string orderNumber);


        Task<BookedHotel> UpdateBookedHotelAsync(BookedHotel bookedHotel);

    }            
}
