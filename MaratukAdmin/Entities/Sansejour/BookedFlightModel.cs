using MaratukAdmin.Dto.Request;

namespace MaratukAdmin.Entities.Sansejour
{
    public class BookedFlightModel
    {
        public List<AddBookedFlight> BookedFlights { get; set; }
        public BookedInvoiceData? BookInvoiceData { get; set; }
    }
}
