using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IBookedHotelRepository
    {
        //Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int agentId);
        Task<List<BookedHotelResponse >> GetAllBookedHotelsAsync(List<AgencyUser> agencyUsers);
        Task<BookedHotel> CreateBookedHotelAsync(BookedHotel bookedHotel, List<BookedHotelGuest> bookedHotelGuests);
    }
}
