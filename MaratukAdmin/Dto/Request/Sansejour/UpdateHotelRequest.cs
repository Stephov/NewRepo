using MaratukAdmin.Entities.Sansejour;
using Swashbuckle.AspNetCore.Annotations;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    //public class UpdateHotelRequest : Hotel
    public class UpdateHotelRequest
    {
        public Hotel hotel {  get; set; }
        //[SwaggerSchema(ReadOnly = true)]
        //public int Id { get; set; }
    }
}
