namespace MaratukAdmin.Entities.Report
{
    public class ReportTotalPreparedData
    {
        public DateTime? DateOfOrder { get; set; }
        public string? OrderNumber { get; set; }
        public string? FlightManager { get; set; }
        public string? HotelManager { get; set; }
        public string? CompanyName { get; set; }
        public string? PassengerName { get; set; }
        public string? PassengerSurName { get; set; }
        public DateTime? FlightStartDate { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? FlightEndDate { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public string? BookStatus { get; set; }
        public double? RoomPrice { get; set; }
        public int? AccomodationDaysCount { get; set; }
        public int? TicketsCount {  get; set; }
        public double? HotelTotal { get; set; }
        public double? HotelTotalAMD { get; set; }
        public string? Rate { get; set; }
    }
}
