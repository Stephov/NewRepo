using System.ComponentModel.DataAnnotations;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class AddHotelImageRequest
    {
        [Required]
        public IFormFile FileContent { get; set; }
        public int HotelId { get; set; }
        public int FileTypeId { get; set; }
        //public string? FileName { get; set; }
        //public string? FilePath { get; set; }
        //public byte[] FileData { get; set; }
        //public string MediaType { get; set; }
    }
}
