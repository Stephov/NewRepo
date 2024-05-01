namespace MaratukAdmin.Entities
{
    public class FlightInfoFunction
    {
        private double _price;

        public int FlightId { get; set; }
        public int PriceBlockId { get; set; }
        public string DepartureCountryName { get; set; }
        public int DepartureCountryId { get; set; }
        public string DepartureCityName { get; set; }
        public int DepartureCityId { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationCountryName { get; set; }
        public int DestinationCountryId { get; set; }
        public string DestinationCityName { get; set; }
        public int DestinationCityId { get; set; }

        public string DestinationAirportName { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
        //public double? Price { get; set;}
        public double? Price
        {
            get => _price;
            set => _price = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
