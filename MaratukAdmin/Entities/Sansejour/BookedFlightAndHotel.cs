using MaratukAdmin.Dto.Request;

namespace MaratukAdmin.Entities.Sansejour
{
    public class BookedFlightAndHotel
    {
        public List<AddBookedFlight> BookedFlights { get; set; }
        public int BookedRoomId { get; set; }
    }
}
