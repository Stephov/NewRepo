namespace MaratukAdmin.Entities.Sansejour
{
    public class HotelResponseModel 
    {
        public Hotel hotel { get; set; }
        public string? hotelCountryName { get; set; }
        public string? hotelCountryNameEng { get; set; }
        public string? hotelCityName { get; set; }
        public string? hotelCityNameEng { get; set; }
        public List<HotelImage>? hotelImages { get; set; }
    }
}
