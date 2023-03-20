using MaratukAdmin.Enums;

namespace MaratukAdmin.Entities
{
    public class Schedule : BaseDbEntity
    {
        public DateTime FlightStartDate { get; set; }
        public DateTime FlightEndDate { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
    }
}
