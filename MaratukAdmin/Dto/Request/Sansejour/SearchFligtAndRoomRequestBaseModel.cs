using System.Drawing.Printing;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchFligtAndRoomRequestBaseModel
    {
        private int _pageSize;
        private int _pageNumber;

        public int FlightOneId { get; set; }
        public int? FlightTwoId { get; set; }
        public DateTime FlightStartDate { get; set; }
        public DateTime? FlightReturnedDate { get; set; }
        public string? Board { get; set; }
        public int RoomAdultCount { get; set; }
        public int? RoomChildCount { get; set; }
        public List<float?>? RoomChildAges { get; set; }

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
        //public SearchFligtAndRoomRequestBaseModel()
        //{
        //    PageNumber = 1;
        //    PageSize = 10;
        //}
    }

}
