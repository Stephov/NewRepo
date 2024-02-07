using System.Drawing.Printing;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchFligtAndRoomRequestBaseModel
    {
        //private int _pageSize;
        //private int _pageNumber;

        public int FlightOneId { get; set; }
        public int? FlightTwoId { get; set; }
        public DateTime FlightStartDate { get; set; }
        public DateTime? FlightReturnedDate { get; set; }
        public List<string>? Board { get; set; }
        public bool LateCheckout { get; set; }
        public List<string>? HotelCodes { get; set; }
        public List<int>? HotelCategoryIds { get; set; }
        public List<int>? HotelCountryIds { get; set; }
        public List<int>? HotelCityIds { get; set; }
        public double? TotalPriceMin { get; set; }
        public double? TotalPriceMax { get; set; }

        public int RoomAdultCount { get; set; }
        public int? RoomChildCount { get; set; }
        public List<float?>? RoomChildAges { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // IN CASE OF PageNumber AND PageSize are NULL or 0 - their values will be set in SQL procedure
        //public int PageNumber
        //{
        //    get { return _pageNumber; }
        //    set { _pageNumber = (value == 0) ? 1 : value; }
        //}
        //public int PageSize
        //{
        //    get { return _pageSize; }
        //    set { _pageSize = (value == 0) ? 10 : value; }
        //}

        //public SearchFligtAndRoomRequestBaseModel()
        //{
        //    PageNumber = 1;
        //    PageSize = 10;
        //}
    }

}
