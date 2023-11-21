namespace MaratukAdmin.Entities
{
    public class SearchResultFunction
    {
        public int FlightId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeUpTo { get; set; }
        public double Bruto { get; set; }
        public int FlightTimeMinute { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get;}

    }

    public class SearchResultFunctionTwoWay 
    {
        public int FlightId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeUpTo { get; set; }
        public double Bruto { get; set; }
        public int FlightTimeMinute { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int PriceBlockId { get; set; }
        public string Airline { get; set; }
        public string FlightValue { get; set; }
    }
}
