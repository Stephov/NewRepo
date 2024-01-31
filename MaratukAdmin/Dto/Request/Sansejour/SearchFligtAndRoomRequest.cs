using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response.Sansejour;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchFligtAndRoomRequest : SearchFligtAndRoomRequestBaseModel
    {
        private int _flightAdultCount;
        private int _flightChildCount;
        private int _flightInfantCount;
        private int _roomTotalCount;
        //private int _pageNumber;
        //private int _pageSize;


        // Flight part
        public int FlightAdult { get { return _flightAdultCount; } } 
        public int FlightChild { get { return _flightChildCount; } }
        public int FlightInfant { get { return _flightInfantCount; } }

        // Room part
        public DateTime? RoomExportDate { get; set; }
        public DateTime? RoomAccomodationDateFrom { get; set; }
        public DateTime? RoomAccomodationDateTo { get; set; }
        public string? Board { get; set; }
        //public int RoomTotalCount { get { return RoomAdultCount + (RoomChildCount ?? 0); } }
        public int RoomTotalCount { get { return _roomTotalCount; } }
        //public int PageNumber { get { return _pageNumber; } }
        //public int PageSize { get { return _pageSize; } }
        public List<string>? RoomHotelCodes { get; set; }

        public SearchFligtAndRoomRequest(SearchFligtAndRoomRequestBaseModel baseModel)
        {

            RoomAccomodationDateFrom = baseModel.FlightStartDate;
            RoomAccomodationDateTo = baseModel.FlightReturnedDate;

            FlightOneId = baseModel.FlightOneId;
            FlightTwoId = baseModel.FlightTwoId;
            FlightStartDate = baseModel.FlightStartDate;
            FlightReturnedDate = baseModel.FlightReturnedDate;
            Board = baseModel.Board;
            RoomAdultCount = baseModel.RoomAdultCount;
            RoomChildCount = baseModel.RoomChildCount;
            RoomChildAges = baseModel.RoomChildAges;
            PageNumber= baseModel.PageNumber;
            PageSize= baseModel.PageSize;
            

            // Define child counts and ages for Flight seach
            if (baseModel.RoomChildAges != null)
            {
                foreach (var man in baseModel.RoomChildAges)
                {
                    if (man <= 2)
                    { _flightInfantCount++; }
                    else if (man > 2 && man <= 12)
                    { _flightChildCount++; }
                    else if (man > 12)
                    { _flightAdultCount++; }
                }
                _flightAdultCount += RoomAdultCount;
            }

            _roomTotalCount = baseModel.RoomAdultCount + (baseModel.RoomChildCount ?? 0);
            //_pageNumber = baseModel.PageNumber;
            //_pageSize = baseModel.PageSize;

        }

    }
}
