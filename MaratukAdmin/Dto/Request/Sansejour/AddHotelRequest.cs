namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class AddHotelRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int HotelCategoryId { get; set; }
    }
}
