using MaratukAdmin.Dto.Request;

namespace MaratukAdmin.Entities
{
    public class PriceBlockServices : BaseDbEntity
    {
        public int DepartureCountryId { get; set; }
        public int DepartureCityId { get; set; }
        public int DestinationCountryId { get; set; }
        public int DestinationCityId { get; set; }
        public int FlightId { get; set; }
        public int PriceBlockId { get; set; }
    }
}
