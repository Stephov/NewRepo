using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    public class SearchFligtAndRoomResponse 
    {
        public double? flightAndRoomTotalPrice { get; set; }
        public FinalFlightSearchResponse flightSearchResponse { get; set; }
        public SyncSejourRate roomSearchResponse { get; set; }

        //public DateTime RoomSyncDate { get; set; }
        //public string? RoomHotelCode { get; set; }
        //public DateTime? RoomHotelSeasonBegin { get; set; }
        //public DateTime? RoomHotelSeasonEnd { get; set; }
        //public string? RoomRecID { get; set; }
        //public DateTime? RoomCreateDate { get; set; }
        //public DateTime? RoomChangeDate { get; set; }
        //public DateTime? RoomAccomodationPeriodBegin { get; set; }
        //public DateTime? RoomAccomodationPeriodEnd { get; set; }
        //public string? Room { get; set; }
        //public string? RoomDesc { get; set; }
        //public string? RoomType { get; set; }
        //public string? RoomTypeDesc { get; set; }
        //public string? RoomBoard { get; set; }
        //public string? RoomBoardDesc { get; set; }
        //public int? RoomPax { get; set; }
        //public int? RoomAdlPax { get; set; }
        //public int? RoomChdPax { get; set; }
        //public string? RoomAccmdMenTypeCode { get; set; }
        //public string? RoomAccmdMenTypeName { get; set; }
        //public int? RoomReleaseDay { get; set; }
        //public string? RoomPriceType { get; set; }
        //public double? RoomPrice { get; set; }
        //public string? RoomWeekendPrice { get; set; }
        //public double? RoomWeekendPercent { get; set; }
        //public string? RoomAccomLengthDay { get; set; }
        //public string? RoomOption { get; set; }
        //public string? RoomSpoNoApply { get; set; }
        //public double? RoomSPOPrices { get; set; }
        //public string? RoomSPODefinit { get; set; }
        //public string? RoomNotCountExcludingAccomDate { get; set; }
    }
}
