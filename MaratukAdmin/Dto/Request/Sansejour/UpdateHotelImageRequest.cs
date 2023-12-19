using MaratukAdmin.Entities.Sansejour;
using System.ComponentModel.DataAnnotations;


using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using System.Linq;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class UpdateHotelImageRequest
    {

        //[Required]
        public IFormFile? FileContent { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        public HotelImage hotelImage { get; set; }
    }
}
