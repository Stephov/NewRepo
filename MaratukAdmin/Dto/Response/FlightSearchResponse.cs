using System.Text.Json.Serialization;

namespace MaratukAdmin.Dto.Response
{
    public class FlightSearchResponse
    {
        private double _totalPrice;
        private double _adultPrice;
        private double _childPrice;
        private double _infantPrice;

        public int FlightId { get; set; }
        public double CostPerTickets { get; set; }
        public double TotalPrice
        {
            get => _totalPrice = (_totalPrice == null) ? 0 : Math.Ceiling((double)_totalPrice);
            set => _totalPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public int NumberOfTravelers { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public double AdultPrice
        {
            get => _adultPrice = (_adultPrice == null) ? 0 : Math.Ceiling((double)_adultPrice);
            set => _adultPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public double ChildPrice
        {
            get => _childPrice = (_childPrice == null) ? 0 : Math.Ceiling((double)_childPrice);
            set => _childPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public double InfantPrice
        {
            get => _infantPrice = (_infantPrice == null) ? 0 : Math.Ceiling((double)_infantPrice);
            set => _infantPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public int CurrencyId { get; set; }
    }

    public class FinalFlightSearchResponse
    {
        private double _totalPrice;
        private double _adultPrice;
        private double _childPrice;
        private double _infantPrice;

        //public FlightSearchResponse OneWay { get; set; }public int GroupId { get; set; }
        public int FlightId { get; set; }
        public double CostPerTickets { get; set; }
        public double TotalPrice 
        {
            get => _totalPrice = (_totalPrice == null) ? 0 : Math.Ceiling((double)_totalPrice);
            set => _totalPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public int NumberOfTravelers { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public double AdultPrice
        {
            get => _adultPrice = (_adultPrice == null) ? 0 : Math.Ceiling((double)_adultPrice);
            set => _adultPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public double ChildPrice
        {
            get => _childPrice = (_childPrice == null) ? 0 : Math.Ceiling((double)_childPrice);
            set => _childPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public double InfantPrice
        {
            get => _infantPrice = (_infantPrice == null) ? 0 : Math.Ceiling((double)_infantPrice);
            set => _infantPrice = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public FlightSearchResponse ReturnedFlight { get; set; }
        public bool IsTwoWay { get; set; }
        public int PriceBlockId { get; set; }
        [JsonIgnore]
        public decimal? Commission { get; set; }
    }
}
