namespace MaratukAdmin.Entities
{
    public class Flight : BaseDbEntity
    {
        public string Name { get; set; }
        public int DepartureCountryId { get; set; }//create country
        public int DepartureCityId { get; set; }//create City
        public int DepartureAirportId { get; set; }//create aeroport vyleta
        public int DestinationCountryId { get; set; }//create country
        public int DestinationCityId { get; set; }//create City
        public int DestinationAirportId { get; set; }//create aeroport prilota
        public int AirlineId { get; set; }//create aviakompania
        public string FlightValue { get; set; }
        public int AircraftId { get; set; }//create vozdushnoye sudno
        public int TripTypeId { get; set; }
        public int TripDays { get; set; }


        public ICollection<Schedule> Schedules { get; set; }
    }
}
