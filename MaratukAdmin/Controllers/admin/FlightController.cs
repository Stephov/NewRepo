﻿using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class FlightController : BaseController
    {

        private readonly IFlightManager _flightManager;
        private readonly IBookedFlightManager _bookedFlightManager;
        public FlightController(IFlightManager flightManager, IBookedFlightManager bookedFlightManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _flightManager = flightManager;
            _bookedFlightManager = bookedFlightManager;
        }


        [HttpGet]
        public async Task<ActionResult> GetFlights()
        {
            var result = await _flightManager.GetAllFlightAsync();

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetFlightById(int id)
        {
            var result = await _flightManager.GetFlightByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("FlightByIds/")]
        public async Task<ActionResult> GetFlightByIds(int departureCountryId, int departureCityId, int destinationCountryId, int destinationCityId)
        {
            var result = await _flightManager.GetFlightByIdsAsync(departureCountryId,departureCityId, destinationCountryId, destinationCityId);

            return Ok(result);
        }


        [HttpGet("info/{id:int}")]
        public async Task<ActionResult> GetFlightIifoById(int id)
        {
            var result = await _flightManager.GetFlightInfoByIdAsync(id);

            return Ok(result);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CreateFlightAsync([FromBody] AddFlightRequest flightRequest)
        {
            try
            {
                //call manager
                var result = await _flightManager.AddFlightAsync(flightRequest);

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


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFlightAsync([FromBody] UpdateFlightRequest flightRequest)
        {
            var result = await _flightManager.UpdateFlightAsync(flightRequest);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFlightAsync(int id)
        {
            var result = await _flightManager.DeleteFlightAsync(id);
            return Ok(result);
        }


        [HttpGet("isFlightNameExist")]
        [AllowAnonymous]
        public async Task<bool> isFlightNameExist(string name)
        {
            var res = await _flightManager.IsFlightNameExistAsync(name);
            return res;
        }

        [HttpPost("BookFlight")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> BookFlight([FromBody] List<AddBookedFlight> addBookedFlight)
        {
            try
            {
                //call manager
                var result = await _bookedFlightManager.AddBookedFlightAsync(addBookedFlight);

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

        [HttpGet("BookFlight/{agentId:int}")]
        [AllowAnonymous]
        public async Task<BookedFlightResponseFinal> GetBookFlightAsync(int agentId)
        {
            var res = await _bookedFlightManager.GetBookedFlightByAgentIdAsync(agentId);
            return res;
        }

        [HttpGet("GetBookFlightByMaratukAgentId/{maratukAgentId:int}")]
        [AllowAnonymous]
        public async Task<BookedFlightResponseFinalForMaratukAgent> GetBookFlightForMaratukAgentAsync(int maratukAgentId, int pageNumber = 1, int pageSize = 10)
        {
            var res = await _bookedFlightManager.GetBookedFlightByMaratukAgentIdAsync(maratukAgentId, pageNumber, pageSize);
            return res;
        }

        [HttpGet("GetBookFlightForAccountat")]
        [AllowAnonymous]
        public async Task<BookedFlightResponseFinalForMaratukAgent> GetBookFlightForMaratukAgentAsync(int pageNumber = 1, int pageSize = 10)
        {
            var res = await _bookedFlightManager.GetBookedFlightForAccAsync(pageNumber, pageSize);
            return res;
        }

        [HttpGet("AllBookFlight/{Itn:int}")]
        [AllowAnonymous]
        public async Task<BookedFlightResponseFinal> GetAllBookFlightAsync(int Itn)
        {
            var res = await _bookedFlightManager.GetBookedFlightAsync(Itn);
            return res;
        }


        [HttpPut("UpdateBookFlightUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBookFlightUserInfoAsync([FromBody] BookedUserInfoForMaratukRequest bookedUserInfoForMaratuk)
        {
            var result = await _bookedFlightManager.UpdateBookedUserInfoAsync(bookedUserInfoForMaratuk);
            return Ok(result);
        }

        [HttpPut("UpdateBookFlightStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBookFlightStatusAsync(string orderNumber,int statusId)
        {
            var result = await _bookedFlightManager.UpdateBookedStatusAsync(orderNumber, statusId);
            return Ok(result);
        }
    }
}
