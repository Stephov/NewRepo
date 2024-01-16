using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    public class BookedHotelResponse
    {
        public BookedHotel? BookedHotel { get; set; }
        public List<BookedHotelGuest>? BookedHotelGuests { get; set; }

    }
}
