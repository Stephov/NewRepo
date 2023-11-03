namespace MaratukAdmin.Entities.Sansejour
{
    public class SyncSejourHotel : BaseDbEntity
    {

        public DateTime SyncDate { get; set; }
        public string? HotelCode { get; set; }
        public string? HotelName { get; set; }
        public string? HotelCategory { get; set; }
        public string? HotelAddress { get; set; }
        public string? HotelRegionCode { get; set; }
        public string? HotelRegion { get; set; }
        public string? HotelTrfRegionCode { get; set; }
        public string? HotelTrfRegion { get; set; }
        public string? AllotmentType { get; set; }
        public string? Currency { get; set; }
        public string? HotelSeason { get; set; }
        public DateTime? HotelSeasonBegin { get; set; }
        public DateTime? HotelSeasonEnd { get; set; }
        public string? HotelType { get; set; }
        public string? ChildAgeCalculationOrder { get; set; }

    }
}
