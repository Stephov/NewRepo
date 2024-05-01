namespace MaratukAdmin.Entities
{
    public class SearchResultFunction
    {
        private double _bruto;

        public int FlightId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeUpTo { get; set; }
        public double Bruto
        {
            get => _bruto;
            set => _bruto = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public int CurrencyId { get; set; }
        public decimal? Comission { get; set; }

    }

    public class SearchResultFunctionTwoWay
    {
        private double _bruto;

        public int FlightId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeUpTo { get; set; }
        public double Bruto
        {
            get => _bruto;
            set => _bruto = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int PriceBlockId { get; set; }
        public string Airline { get; set; }
        public string FlightValue { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public int CurrencyId { get; set; }
        public decimal? Comission { get; set; }
    }
}
