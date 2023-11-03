namespace MaratukAdmin.Entities.Sansejour
{
    public class SyncSejourSpecialOffer : BaseDbEntity
    {
        public DateTime SyncDate { get; set; }
        public string? HotelCode { get; set; }
        public DateTime? HotelSeasonBegin { get; set; }
        public DateTime? HotelSeasonEnd { get; set; }
        public string? SpoCode { get; set; }
        public int? SpoNo { get; set; }
        public DateTime? SalePeriodBegin { get; set; }
        public DateTime? SalePeriodEnd { get; set; }
        public string? SpecialCode { get; set; }
    }
}
