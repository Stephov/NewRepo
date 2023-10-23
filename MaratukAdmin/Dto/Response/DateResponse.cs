namespace MaratukAdmin.Dto.Response
{
    public class DateResponse
    {
        public List<RoundTripResponse>? Date { get; set; }
        public List<ManualTripResponse>? Manual { get; set; }
    }

    public class RoundTripResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
        public double? Price { get; set; }
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }

    public class ManualTripResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Price { get; set; }
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DayOfWeek { get; set; }

    }
}
