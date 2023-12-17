using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Managers.Concrete.Sansejour;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = "AdminScheme")]
    public class HotelImageController : BaseController
    {
        private readonly IHotelImagesManager _hotelImagesManager;
        public HotelImageController(IHotelImagesManager hotelImagesManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _hotelImagesManager = hotelImagesManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetImages()
        {
            var result = await _hotelImagesManager.GetAllHotelImagesAsync();

            return Ok(result);
        }

        [HttpGet("GetHotelImageByImageId/{id:int}")]
        public async Task<ActionResult> GetHotelImageByImageIdAsync(int id)
        {
            var result = await _hotelImagesManager.GetHotelImageByImageIdAsync(id);

            return Ok(result);
        }

        [HttpGet("GetHotelImagesByHotelId/")]
        public async Task<ActionResult> GetHotelImagesByHotelId(int hotelId)
        {
            var result = await _hotelImagesManager.GetHotelImagesByHotelIdAsync(hotelId);

            return Ok(result);
        }

        [HttpGet("GetHotelImagesByHotelCode/")]
        public async Task<ActionResult> GetHotelImagesByHotelCode(string code)
        {
            var result = await _hotelImagesManager.GetHotelImagesByHotelCodeAsync(code);

            return Ok(result);
        }

        //[HttpGet("GetHotelImagesByHotelCodeMock/")]
        //public async Task<ActionResult> GetHotelByCodeMock(string code)
        //{
        //    var result = await _hotelImagesManager.GetHotelByCodeMockAsync(code);

        //    return Ok(result);
        //}


        [HttpPut]
        //[HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotelImage([FromForm] UpdateHotelImageRequest hotelImageRequest)
        {
            if (hotelImageRequest.FileContent == null || hotelImageRequest.FileContent.Length == 0)
                return BadRequest("Invalid file");

            var result = await _hotelImagesManager.UpdateHotelImageAsync(hotelImageRequest);

            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddHotelImage([FromForm] AddHotelImageRequest hotelRequest)
        {
            try
            {
                var result = await _hotelImagesManager.AddHotelImageAsync(hotelRequest);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

    }
}
