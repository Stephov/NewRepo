namespace MaratukAdmin.Dto.Response
{
    public class DateResponse
    {
        public List<RoundTripResponse>? Date { get; set; }
        public List<ManualTripResponse>? Manual { get; set; }
    }

    public class RoundTripResponse
    {
        private double? _price;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
        public double? Price
        {
            get => _price = (_price == null) ? 0 : Math.Ceiling((double)_price);
            set => _price = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }

    public class ManualTripResponse
    {
        private double? _price;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Price
        {
            get => _price = (_price == null) ? 0 : Math.Ceiling((double)_price);
            set => _price = (value == null) ? 0 : Math.Ceiling((double)value);
        }
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DayOfWeek { get; set; }

    }
}
