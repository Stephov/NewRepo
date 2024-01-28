namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchRoomRequest
    {
        private int _pageSize;
        private int _pageNumber;
        
        public DateTime? ExportDate { get; set; }
        public DateTime? AccomodationDateFrom { get; set; }
        public DateTime? AccomodationDateTo { get; set; }
        public string? Board { get; set; }
        public int AdultCount { get; set; }
        public int? ChildCount { get; set; }
        public int TotalCount { get { return AdultCount + (ChildCount ?? 0); } }
        public List<string>? HotelCodes { get; set; }
        public List<float?>? ChildAges { get; set; }
        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = (value == 0) ? 1 : value; }
        }
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value == 0) ? 10 : value; }
        }
        //public SearchRoomRequest()
        //{
        //    PageNumber = 1;
        //    PageSize = 10;
        //}
    }
}
