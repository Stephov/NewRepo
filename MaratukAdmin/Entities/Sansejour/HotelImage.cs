namespace MaratukAdmin.Entities.Sansejour
{
    public class HotelImage : BaseDbEntity
    {
        public int HotelId { get; set; }
        public int? FileTypeId { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public byte[]? FileData { get; set; }
        public string? MediaType { get; set; }

    }
}
