namespace MaratukAdmin.Entities.Sansejour
{
    public class SyncSejourRate : BaseDbEntity
    {
        //public DateTime RowDate { get; set; }
        public DateTime SyncDate { get; set; }
        public string? HotelCode { get; set; }
        public DateTime? HotelSeasonBegin { get; set; }
        public DateTime? HotelSeasonEnd { get; set; }
        public string? RecID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime? AccomodationPeriodBegin { get; set; }
        public DateTime? AccomodationPeriodEnd { get; set; }
        public string? Room { get; set; }
        public string? RoomDesc { get; set; }
        public string? RoomType { get; set; }
        public string? RoomTypeDesc { get; set; }
        public string? Board { get; set; }
        public string? BoardDesc { get; set; }
        public int? RoomPax { get; set; }
        public int? RoomAdlPax { get; set; }
        public int? RoomChdPax { get; set; }
        public string? AccmdMenTypeCode { get; set; }
        public string? AccmdMenTypeName { get; set; }
        public int? ReleaseDay { get; set; }
        public string? PriceType { get; set; }
        public double? Price { get; set; }
        ////public decimal? Percent { get; set; }
        public string? WeekendPrice { get; set; }
        public double? WeekendPercent { get; set; }
        public string? AccomLengthDay { get; set; }
        public string? Option { get; set; }
        public string? SpoNoApply { get; set; }
        public double? SPOPrices { get; set; }
        public string? SPODefinit { get; set; }
        public string? NotCountExcludingAccomDate { get; set; }
    }
}
