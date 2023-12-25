namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class AddHotelRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Country { get; set; }
        public int? City { get; set; }
        public int HotelCategoryId { get; set; }
        public int? IsCruise { get; set; }
        public string? Address { get; set; }
        public string? GpsLatitude { get; set; }
        public string? GpsLongitude { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Site { get; set; }
        public string? Description { get; set; }
    }
}
