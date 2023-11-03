namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchRoomRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public DateTime? ExportDate { get; set; }
        public DateTime? AccomodationDateFrom { get; set; }
        public DateTime? AccomodationDateTo { get; set; }
        public int AdultCount { get; set; }
        public int? ChildCount { get; set; }
        public int TotalCount { get { return AdultCount + (ChildCount ?? 0); } }
        public List<string>? HotelCodes { get; set; }
        public List<float?>? ChildAges { get; set; }

    }
}
