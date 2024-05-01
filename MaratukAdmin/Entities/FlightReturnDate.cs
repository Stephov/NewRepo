namespace MaratukAdmin.Entities
{
    public class FlightReturnDate
    {
        private double _price;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
        public double? Price
        {
            get => _price;
            set => _price = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
