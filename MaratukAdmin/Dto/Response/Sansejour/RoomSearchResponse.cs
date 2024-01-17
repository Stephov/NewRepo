using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    public class RoomSearchResponse //: RoomSearchResponseLowestPrices
    {
        public int Id { get; set; }
        public DateTime SyncDate { get; set; }
        public int HotelId { get; set; }
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
        public string? WeekendPrice { get; set; }
        public double? WeekendPercent { get; set; }
        public string? AccomLengthDay { get; set; }
        public string? Option { get; set; }
        public string? SpoNoApply { get; set; }
        public double? SPOPrices { get; set; }
        public string? SPODefinit { get; set; }
        public string? NotCountExcludingAccomDate { get; set; }


        public string? HotelName { get; set; }
        public int? HotelCategoryId { get; set; }
        //public int? HotelFileTypeId { get; set; }
        public string? FilePath { get; set; }

        // *****************************************

        //public string? HotelCountryName { get; set; }
        //public string? HotelCountryNameEng { get; set; }
        //public string? HotelCityName { get; set; }
        //public string? HotelCityNameEng { get; set; }

        //public byte? HotelIsCruise { get; set; }
        //public string? HotelAddress { get; set; }
        //public string? HotelGpsLatitude { get; set; }
        //public string? HotelGpsLongitude { get; set; }
        //public string? HotelPhoneNumber { get; set; }
        //public string? HotelFax { get; set; }
        //public string? HotelEmail { get; set; }
        //public string? HotelSite { get; set; }
        //public DateTime? HotelCheckIn { get; set; }
        //public DateTime? HotelCheckOut { get; set; }
        //public string? HotelDescription { get; set; }

    }
}
