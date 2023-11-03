namespace MaratukAdmin.Entities.Sansejour
{
    public class SyncSejourSpoAppOrder : BaseDbEntity
    {
        public DateTime SyncDate { get; set; }
        public string? HotelCode { get; set; }
        public DateTime? HotelSeasonBegin { get; set; }
        public DateTime? HotelSeasonEnd { get; set; }
        public string? SpoCode { get; set; }
        public int? SpoOrder { get; set; }
    }
}
