namespace MaratukAdmin.Entities.Report
{
    public class ReportFlightInfo
    {
        public DateTime FlightDate {  get; set; }
        public string FlightNumber { get; set; }
        public int WaitingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int ConfirmedByAccountantCount { get; set; }
        public int InvoiceSentCount { get; set; }
        public int PaidPartiallyCount { get; set; }
        public int PaidFullCount { get; set; }
        public int TicketSentCount { get; set; }
        public int TicketConfirmedCount { get; set; }
        public double TotalPrice { get; set; }
        public string Currency { get; set; }
        public int TotalPassCount { get; set; }

    }
}
