namespace MaratukAdmin.Dto.Response
{
    public class FlightSearchResponse
    {
        public int FlightId { get; set; }
        public double CostPerTickets { get; set; }
        public double TotalPrice { get; set; }     
        public int NumberOfTravelers { get; set; }
        public string DepartureAirportCode { get; set;}
        public string DestinationAirportCode { get; set;}
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public double AdultPrice { get; set; }
        public double ChildPrice { get; set; }
        public double InfantPrice { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public int CurrencyId { get; set; }
    }

    public class FinalFlightSearchResponse
    {
        //public FlightSearchResponse OneWay { get; set; }public int GroupId { get; set; }
        public int FlightId { get; set; }
        public double CostPerTickets { get; set; }
        public double TotalPrice { get; set; }
        public int NumberOfTravelers { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public double AdultPrice { get; set; }
        public double ChildPrice { get; set; }
        public double InfantPrice { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public FlightSearchResponse ReturnedFlight { get; set; }
        public bool IsTwoWay { get; set; }
        public decimal? Commission { get; set; }
    }
}
