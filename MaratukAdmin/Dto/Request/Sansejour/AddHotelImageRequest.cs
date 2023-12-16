namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class AddHotelImageRequest
    {
        public int HotelId { get; set; }
        public int FileTypeId { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }
}
