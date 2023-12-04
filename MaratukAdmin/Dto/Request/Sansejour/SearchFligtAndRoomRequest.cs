namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchFligtAndRoomRequest
    {
        // Flight part
        public int FlightOneId { get; set; }
        public int? FlightTwoId { get; set; }
        public DateTime FlightStartDate { get; set; }
        public DateTime? FlightReturnedDate { get; set; }
        public int FlightAdult { get; set; }
        public int FlightChild { get; set; }
        public int FlightInfant { get; set; }

        // Room part
        public int RoomPageNumber { get; set; }
        public int RoomPageSize { get; set; } = 10;
        public DateTime? RoomExportDate { get; set; }
        public DateTime? RoomAccomodationDateFrom { get; set; }
        public DateTime? RoomAccomodationDateTo { get; set; }
        public int RoomAdultCount { get; set; }
        public int? RoomChildCount { get; set; }
        public int RoomTotalCount { get { return RoomAdultCount + (RoomChildCount ?? 0); } }
        public List<string>? RoomHotelCodes { get; set; }
        public List<float?>? RoomChildAges { get; set; }
    }
}
