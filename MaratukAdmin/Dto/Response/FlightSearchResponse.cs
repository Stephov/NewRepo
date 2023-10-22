namespace MaratukAdmin.Dto.Response
{
    public class FlightSearchResponse
    {
        public int GroupId { get; set; }
        public int FlightId { get; set; }
        public double CostPerTickets { get; set; }
        public double TotalPrice { get; set; }     
        public int NumberOfTravelers { get; set; }
        public int DirectTimeToMinute { get; set; }
        public string DepartureAirportCode { get; set;}
        public string DestinationAirportCode { get; set;}

        public double AdultPrice { get; set; }
        public double ChildPrice { get; set; }
        public double InfantPrice { get; set; }
    }
}
