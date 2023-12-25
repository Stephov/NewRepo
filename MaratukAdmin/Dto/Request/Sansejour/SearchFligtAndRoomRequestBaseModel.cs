namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SearchFligtAndRoomRequestBaseModel
    {
        public int FlightOneId { get; set; }
        public int? FlightTwoId { get; set; }
        public DateTime FlightStartDate { get; set; }
        public DateTime? FlightReturnedDate { get; set; }
        public int RoomAdultCount { get; set; }
        public int? RoomChildCount { get; set; }
        public List<float?>? RoomChildAges { get; set; }
    }
}
