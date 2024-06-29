namespace MaratukAdmin.Entities.Report
{
    public class ReportFlightInfoPreparedData
    {
        public DateTime FlightDate { get; set; }
        public string FlightNumber { get; set; }
        public int StartFlightId { get; set; }
        public int MaratukAgentStatusId { get; set; }
        public string MaratukAgentStatusName { get; set; }
        public int StatusesCount { get; set; }
        public double TotalPrice { get; set; }
        public string Currency { get; set; }
        public int PassengersCount { get; set; }
    }
}
