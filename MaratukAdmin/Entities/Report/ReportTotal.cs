namespace MaratukAdmin.Entities.Report
{
    public class ReportTotal
    {
        public DateTime? DateOfOrder { get; set; }
        public string? OrderNumber { get; set; }
        public string? TourManager { get; set; }
        public string? CompanyName { get; set; }
        public string? PassengerName { get; set; }
        public DateTime? FlightStartDate { get; set; }
        public string? DepartureTime { get; set; }
        public DateTime? FlightEndDate { get; set; }
        public string? ArrivalTime { get; set; }
        public string? BookStatus { get; set; }
        public double? RoomPrice { get; set; }
        public int? AccomodationDaysCount { get; set; }
        public int? TicketsCount { get; set; }
        public double? HotelTotal { get; set; }
        public string? Rate { get; set; }
        public double? Total { get; set; }
        public string? PaidUnpaid { get; set; }
        public string? Confirm { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
