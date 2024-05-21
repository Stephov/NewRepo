namespace MaratukAdmin.Entities.Report
{
    public class ReportTouristInfoFlight : IReportTouristInfo
    {
        public string? ToureTypeId { get; set; }
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

        public string? Tiitle { get; set; }
        public DateTime? Dob {  get; set; }
        public double Paid { get; set; }
        public string? Currency { get; set; }
        public double? CurrencyRate { get; set; }
        public string? BookStatus { get; set; }
    }
}
