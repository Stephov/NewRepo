namespace MaratukAdmin.Dto.Response.Sansejour
{
    public class SearchFligtAndRoomLowestPricesResponse
    {
        public double? flightAndRoomTotalPrice { get; set; }
        public FinalFlightSearchResponse flightSearchResponse { get; set; }
        public RoomSearchResponseLowestPrices roomSearchResponse { get; set; }
    }
}
