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
        public double HotelTotalPrice { get; set; }
        public string? Board { get; set; }
        public string? BoardDesc { get; set; }
        //public int HotelAgentId { get; set; }              // Record from Users table
        //public int MaratukHotelAgentId { get; set; }       // Record from AgencyUser table
        public DateTime AccomodationStartDate { get; set; }
        public DateTime? AccomodationEndDate { get; set; }
        public bool LateCheckout { get; set; }
        public BookedInvoiceData? BookInvoiceData { get; set; }
    }
}
