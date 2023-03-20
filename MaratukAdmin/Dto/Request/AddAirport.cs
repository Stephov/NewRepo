using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class AddAirport
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int CityId { get; set; }
    }
}
