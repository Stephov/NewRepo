using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class GetHotelsByCountryAndCityListRequest
    {
        public bool IncludeImages { get; set; } = true;
        public List<int>? countryIds { get; set; }
        public List<int>? cityIds { get; set; }
    }
}
