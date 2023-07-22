using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class AddPriceBlockServicesRequest
    {
        public int DepartureCountryId { get; set; }
        public int DepartureCityId { get; set; }
        public int DestinationCountryId { get; set; }
        public int DestinationCityId { get; set; }
        public int FligthId { get; set; }
        public int PriceBlockId { get; set; }

    }
}
