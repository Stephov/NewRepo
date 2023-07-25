using MaratukAdmin.Dto.Request;

namespace MaratukAdmin.Dto.Response
{
    public class PriceBlockServicesResponse
    {
        public int PriceBlockServicesId { get; set; }
        public string DepartureCountryName { get; set; }
        public string DepartureCityName { get; set; }
        public string DestinationCountryName { get; set; }
        public string DestinationCityName { get; set; }
        public string FlightName { get; set; }
        public string FlightValue { get; set; }
    }


}

