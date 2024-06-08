namespace MaratukAdmin.Entities.Report
{
    public class ReportTouristInfoFlight
    {
        public string? ToureTypeId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime? Date { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? Manager { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string? DepartureTime { get; set; }
        public string? ArrivalTime { get; set; }
        public string? EndFlightDepartureTime { get; set; }
        public string? EndFlightArrivalTime { get; set; }

        public string? DepartureFlightValue { get; set; }
        public string? ArrivalFlightValue { get; set; }
        public string? DepartureCountryName { get; set; }
        public string? ArrivalCountryName { get; set; }
        public string? DepartureCityName { get; set; }
        public string? ArrivalCityName { get; set; }
        public string? DepartureAirportName { get; set; }
        public string? ArrivalAirportName { get; set; }

        public string? Tiitle { get; set; }
        public DateTime? Dob { get; set; }
        public double Paid { get; set; }
        public double TotalPrice { get; set; }
        public string? Currency { get; set; }
        public double? CurrencyRate { get; set; }
        public string? BookStatus { get; set; }
    }
}
