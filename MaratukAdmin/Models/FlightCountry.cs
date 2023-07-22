namespace MaratukAdmin.Models
{
    public class FlightCountry
    {
        public int DepartureCountryId { get; set; }
        public List<int> DepartureCityIds { get; set; }

    }


    public class FlightCountryResponse
    {
        public int DepartureCountryId { get; set; }
        public string CountryName { get; set; }
        public List<FlightCity> DepartureCity { get; set; }

    }
}
