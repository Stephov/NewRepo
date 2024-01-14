using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IBookedHotelRepository
    {
        //Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int agentId);
        //Task<List<BookedFlight>> GetAllBookedFlightAsync(List<AgencyUser> agencyUsers);
        Task<BookedHotel> CreateBookedHotelAsync(BookedHotel bookedHotel, List<BookedHotelGuest> bookedHotelGuests);
    }
}
