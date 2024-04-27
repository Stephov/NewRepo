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
    public class HotelController : BaseController
    {
        private readonly IHotelManager _hotelManager;
        private readonly IHotelBoardManager _hotelBoardManager;
        private readonly IBookedHotelManager _bookedHotelManager;

        public HotelController(IHotelManager hotelManager,
                                IHotelBoardManager hotelBoardManager,
                                IBookedHotelManager bookedHotelManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _hotelManager = hotelManager;
            _hotelBoardManager = hotelBoardManager;
            _bookedHotelManager = bookedHotelManager;
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

        [HttpGet("GetHotelByCode/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHotelByCode(string code)
        {
            var result = await _hotelManager.GetHotelByCodeAsync(code);

            return Ok(result);
        }

        [HttpGet("GetHotelByCodeMock/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHotelByCodeMock(string code)
        {
            var result = await _hotelManager.GetHotelByCodeMockAsync(code);

            return Ok(result);
        }

        [HttpGet("GetHotelBoardByCode/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHotelBoardByCode(string code)
        {
            var result = await _hotelBoardManager.GetHotelBoardByCodeAsync(code);

            return Ok(result);
        }

        [HttpGet("GetAllHotelBoards/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllHotelBoards()
        {
            var result = await _hotelBoardManager.GetAllHotelBoardsAsync();

            return Ok(result);
        }

        [HttpGet("GetHotelsByCountryAndCityId/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHotelsByCountryAndCityId(bool includeImages = true, int? countryId = null, int? cityId = null)
        {
            try
            {
                var result = await _hotelManager.GetHotelsByCountryIdAndCityIdAsync(includeImages, countryId, cityId);

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


        [HttpPost("GetHotelsByCountryAndCityList/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHotelsByCountryIdListAndCityIdList([FromBody] GetHotelsByCountryAndCityListRequest request)
        {
            try
            {
                var result = await _hotelManager.GetHotelsByCountryIdListAndCityIdListAsync(request);

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

        //[HttpGet("GetHotelByCountryAndCityName/")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> GetHotelByCountryAndCityName(List<string> countryNames, List<string> cityNames)
        //{
        //    var result = await _hotelManager.GetHotelsByCountryAndCityNameAsync(countryNames, cityNames);

        //    return Ok(result);
        //}

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotelAsync([FromBody] UpdateHotelRequest hotelRequest)
        {
            var result = await _hotelManager.UpdateHotelAsync(hotelRequest);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddHotel([FromBody] AddHotelRequest hotelRequest)
        {
            try
            {
                var result = await _hotelManager.AddHotelAsync(hotelRequest);

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


        [HttpGet("RefreshHotelList/")]
        public async Task<IActionResult> RefreshHotelList()
        {
            var result = await _hotelManager.RefreshHotelList();
            return Ok(result);
            //return Ok();
        }

        [HttpPost("BookHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> BookHotel([FromBody] AddBookHotelRequest addBookHotelRequest)
        {
            try
            {
                var result = await _bookedHotelManager.AddBookedHotelAsync(addBookHotelRequest);
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
