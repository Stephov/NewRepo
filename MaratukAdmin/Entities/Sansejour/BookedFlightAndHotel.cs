using MaratukAdmin.Dto.Request;

namespace MaratukAdmin.Entities.Sansejour
{
    public class BookedFlightAndHotel
    {
        public List<AddBookedFlight> BookedFlights { get; set; }
        public int SejourRateId { get; set; }
        public string? Room { get; set; }
        public string? RoomType { get; set; }
        public int HotelId { get; set; }
        public string? HotelCode { get; set; }
        public double Price { get; set; }
        public string? Board { get; set; }
        public string? BoardDesc { get; set; }
        
    }
}
