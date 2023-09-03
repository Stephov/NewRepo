namespace MaratukAdmin.Dto.Response
{
    public class FlightInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartureCountryId { get; set; }
        public int DepartureCityId { get; set; }
        public int DepartureAirportId { get; set; }
        public int DestinationCountryId { get; set; }
        public int DestinationCityId { get; set; }
        public int DestinationAirportId { get; set; }
        public int AirlineId { get; set; }
        public string FlightValue { get; set; }
        public int AircraftId { get; set; }
        public int TripTypeId { get; set; }
        public int TripDays { get; set; }

        public List<ScheduleInfoResponse> schedules { get; set; }
    }
}
