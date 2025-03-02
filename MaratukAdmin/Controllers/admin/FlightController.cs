﻿using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = "AdminScheme")]
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
            var result = await _flightManager.GetFlightByIdsAsync(departureCountryId, departureCityId, destinationCountryId, destinationCityId);

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
        public async Task<BookedFlightResponseFinal> GetBookFlightAsync(int agentId)
        {
            var res = await _bookedFlightManager.GetBookedFlightByAgentIdAsync(agentId);
            return res;
        }

        [HttpGet("GetBookFlightByMaratukAgentId/{maratukAgentId:int}")]
        public async Task<BookedFlightResponseFinalForMaratukAgent> GetBookFlightForMaratukAgentAsync(int roleId, int maratukAgentId, int pageNumber = 1, int pageSize = 10)
        {
            var res = await _bookedFlightManager.GetBookedFlightByMaratukAgentIdAsync(roleId, maratukAgentId, pageNumber, pageSize);
            return res;
        }

        [HttpGet("SearchBookFlight")]
        public async Task<BookedFlightResponseFinalForMaratukAgent> SearchBookFlightAsync(int userId, int roleId, string? searchText, int? status, DateTime? startDate = null, DateTime? endDate = null, int pageNumber = 1, int pageSize = 10)
        {
            var res = await _bookedFlightManager.SearchBookedFlightAsync(userId, roleId, searchText, status, pageNumber, pageSize, startDate, endDate);
            return res;
        }

        [HttpGet("SearchBookFlightByMaratukAgentId/{maratukAgentId:int}")]
        public async Task<BookedFlightResponseFinalForMaratukAgent> SearchBookFlightForMaratukAgentAsync(int roleId, int maratukAgentId, string? searchText, int? status, DateTime? startDate = null, DateTime? endDate = null, int pageNumber = 1, int pageSize = 10)
        {
            var res = await _bookedFlightManager.SearchBookedFlightByMaratukAgentIdAsync(roleId, maratukAgentId, searchText, status, pageNumber, pageSize, startDate, endDate);
            return res;
        }

        [HttpGet("GetBookFlightForAccountat")]
        public async Task<BookedFlightResponseFinalForMaratukAgent> GetBookFlightForMaratukAgentAsync(int pageNumber = 1, int pageSize = 10)
        {
            var res = await _bookedFlightManager.GetBookedFlightForAccAsync(pageNumber, pageSize);
            return res;
        }

        [HttpGet("SearchBookFlightForAccountat")]
        public async Task<BookedFlightResponseFinalForMaratukAgent> SearchBookFlightForMaratukAgentAsync(string? searchText, int? status, DateTime? startDate = null, DateTime? endDate = null, int pageNumber = 1, int pageSize = 10)
        {
            var res = await _bookedFlightManager.SearchBookedFlightForAccAsync(pageNumber, pageSize, searchText, status, startDate, endDate);
            return res;
        }

        [HttpGet("AllBookFlight/{Itn:int}")]
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
        public async Task<IActionResult> UpdateBookFlightStatusAsync(string orderNumber, int statusId, int role, double? totalPrice, string? comment)
        {
            var result = await _bookedFlightManager.UpdateBookedStatusAsync(orderNumber, statusId, role, totalPrice, comment);
            return Ok(result);
        }

        [HttpPost("SetTicketNumberToBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SetTicketNumberToBook([FromBody] SetTicketNumberToBookRequest request)
        {
            try
            {
                var result = await _bookedFlightManager.SetTicketNumberToBookAsync(request);

                if (result != null && result.StatusCode == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Something went wrong");
                }
                
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
