namespace MaratukAdmin.Entities
{
    public class FlightInfoFunction
    {
        public int FlightId { get; set; }
        public string DepartureCountryName { get; set; }
        public string DepartureCityName { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationCountryName { get; set; }
        public string DestinationCityName { get; set; }
        public string DestinationAirportName { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
        public double? Price { get; set;}
    }
}
