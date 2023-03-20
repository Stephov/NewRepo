using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class UpdateFlightRequest
    {
        public int Id { get; set; }
        public int DepartureCountryId { get; set; }
        public int DepartureCityId { get; set; }
        public int DepartureAirportId { get; set; }
        public int DestinationCountryId { get; set; }
        public int DestinationCityId { get; set; }
        public int DestinationAirportId { get; set; }
        public int AirlineId { get; set; }
        public int FlightValue { get; set; }
        public int AircraftId { get; set; }

        public ICollection<ScheduleRequest> Schedules { get; set; }
    }
}
