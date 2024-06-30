namespace MaratukAdmin.Entities.Report
{
    public class ReportSalesByManagerPreparedData
    {
        public DateTime? Date { get; set; }
        public string? OrderNumber { get; set; }
        public string? BookStatus { get; set; }
        public string? AgencyName { get; set; }
        public string? PassengerName { get; set; }
        public string? PassengerSurName { get; set; }
        public double? PassengersCount { get; set; }
        public string? HotelName { get; set; }
        public double? TicketsCostTotal { get; set; }
        public string? Rate {  get; set; }
        public double? TicketsCostInAMD { get; set; }
        public double? HotelCostInAMD { get; set; }
        public DateTime? TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }
        public string? Direction1 { get; set; }
        public string? Direction2 { get; set; }
        public string? ManagerName { get; set; }
        public double? TicketsCount { get; set; }
    }
}
