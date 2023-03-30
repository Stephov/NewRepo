namespace MaratukAdmin.Dto.Response
{
    public class FlightInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartureCountry { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureAirport { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationAirport { get; set; }
        public string Airline { get; set; }
        public string FlightValue { get; set; }
        public string Aircraft { get; set; }

        public List<ScheduleInfoResponse> scheduleInfos { get; set; }
    }
}
