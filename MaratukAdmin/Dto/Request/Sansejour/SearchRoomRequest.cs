namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchRoomRequest
    {
        //private int _pageSize;
        //private int _pageNumber;
        
        public DateTime? ExportDate { get; set; }
        public DateTime? AccomodationDateFrom { get; set; }
        public DateTime? AccomodationDateTo { get; set; }
        public List<string>? Board { get; set; }
        public bool LateCheckout { get; set; }
        public List<string>? HotelCodes { get; set; }
        public List<int>? HotelCategoryIds { get; set; }
        public List<int>? HotelCountryIds { get; set; }
        public List<int>? HotelCityIds { get; set; }
        public double? TotalPriceMin { get; set; }
        public double? TotalPriceMax { get; set; }
        public int AdultCount { get; set; }
        public int? ChildCount { get; set; }
        public int TotalCount { get { return AdultCount + (ChildCount ?? 0); } }
        public List<float?>? ChildAges { get; set; }

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

        //public SearchRoomRequest()
        //{
        //    PageNumber = 1;
        //    PageSize = 10;
        //}
    }
}
