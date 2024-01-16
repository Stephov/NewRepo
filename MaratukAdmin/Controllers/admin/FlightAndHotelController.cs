using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Managers.Abstract;
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
    public class FlightAndHotelController : BaseController
    {
        private readonly IBookedFlightAndHotelManager _bookedFlightAndHotelManager;

        public FlightAndHotelController(IBookedFlightAndHotelManager bookedFlightAndHotelManager,
                                        JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _bookedFlightAndHotelManager = bookedFlightAndHotelManager;
        }

        [HttpPost("BookFlightAndHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //public async Task<ActionResult> BookFlight([FromBody] List<AddBookedFlight> addBookedFlight ) //, AddBookHotelRequest addBookHotelRequest)
        public async Task<ActionResult> BookFlight([FromBody] BookedFlightAndHotel bookedFlightAndHotel) //, AddBookHotelRequest addBookHotelRequest)
        {
            try
            {
                var result = await _bookedFlightAndHotelManager.AddBookedFlightAndHotelAsync(bookedFlightAndHotel);
                //var result1 = await _bookedHotelManager.AddBookedHotelAsync(addBookHotelRequest);

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

        
        [HttpGet("BookFlightAndHotel/{Itn:int}")]
        [AllowAnonymous]
        public async Task<List<BookedHotelResponse>> GetAllBookedFlightsAndHotelsAsync(int Itn)
        {
            var res = await _bookedFlightAndHotelManager.GetBookedFlightsAndHotelsAsync(Itn);
            return res;
        }
    }
}
