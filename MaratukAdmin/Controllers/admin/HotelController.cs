using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class HotelController : BaseController
    {
        private readonly IHotelManager _hotelManager;
        public HotelController(IHotelManager hotelManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _hotelManager = hotelManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetHotels()
        {
            var result = await _hotelManager.GetAllHotelsAsync();

            return Ok(result);
        }

        [HttpGet("GetHotelById/{id:int}")]
        public async Task<ActionResult> GetHotelById(int id)
        {
            var result = await _hotelManager.GetHotelByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("RefreshHotelList/")]
        public async Task<IActionResult> RefreshHotelList()
        {
            var result = await _hotelManager.RefreshHotelList();
            return Ok(result);
        }
    }
}
