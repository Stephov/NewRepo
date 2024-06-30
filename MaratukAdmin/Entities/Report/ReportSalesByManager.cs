namespace MaratukAdmin.Entities.Report
{
    public class ReportSalesByManager
    {
        public DateTime? Date { get; set; }
        public string? OrderNumber { get; set; }
        public string? BookStatus { get; set; }
        public string? AgencyName { get; set; }
        public string? PassengerName { get; set; }
        public double? CostPerTicketInCurrency { get; set; }
        public string? HotelName { get; set; }
        public double? TicketsCostTotal { get; set; }
        public string? Rate {  get; set; }
        public double? TicketsCostInAMD { get; set; }
        public double? HotelCostInAMD { get; set; }
        public double? TotalInAMD { get; set; }
        public string? Dates { get; set; }
        public string? Direction { get; set; }
        public string? ManagerName { get; set; }
        public double? TicketsCount { get; set; }
    }
}
