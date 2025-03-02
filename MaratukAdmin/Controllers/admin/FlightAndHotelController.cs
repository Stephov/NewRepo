﻿using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = "AdminScheme")]
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
        public async Task<ActionResult> BookFlightAndHotel([FromBody] BookedFlightAndHotel bookedFlightAndHotel) //, AddBookHotelRequest addBookHotelRequest)
        {
            try
            {
                var result = await _bookedFlightAndHotelManager.AddBookedFlightAndHotelAsync(bookedFlightAndHotel);

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

        [HttpPost("PayForBookedFlightAndHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> PayForBookedFlightAndHotel([FromBody] PayForBookedFlightAndHotelRequest payForBookedFlightAndHotel)
        {
            try
            {
                var result = await _bookedFlightAndHotelManager.PayForBookedFlightAndHotelAsync(payForBookedFlightAndHotel);

                return Ok(result);
            }
            catch (IncorrectDataException ex)
            { return BadRequest(ex.Message); }
            catch (ArgumentException ex)
            { return Forbid(ex.Message); }
            catch (Exception)
            { return BadRequest("Something went wrong"); }
        }

        //[HttpGet("GetBookedFlight/{Itn:int}")]
        //[AllowAnonymous]
        //public async Task<List<BookedHotelResponse>> GetAllBookedFlightsAndHotelsAsync(int Itn)
        //{
        //    var res = await _bookedFlightAndHotelManager.GetBookedFlightsAsync(Itn);
        //    return res;
        //}

        [HttpGet("GetBookedInfoFlighPart")]
        [AllowAnonymous]
        //public async Task<List<BookedInfoFlightPartResponse>> GetBookedInfoFlighPartAsync(int countryId, int cityId, int agentId, [Required] int startFlightId, // int endFlightId,
        public async Task<IActionResult> GetBookedInfoFlighPart(int countryId, int cityId, int agentId, [Required] int startFlightId, // int endFlightId,
                                                                                [Required] DateTime startDate, [Required] DateTime endDate, bool groupByFlight = false)
        {
            BookedInfoFlightPartRequest request = new()
            {
                AgentId = agentId,
                MaratukAgentId = agentId,
                CityId = cityId,
                CountryId = countryId,
                StartDate = startDate,
                EndDate = endDate,
                StartFlightId = startFlightId,
                GroupByStartFlightId = groupByFlight
                //EndFlightId = endFlightId
            };

            var flightPart = await _bookedFlightAndHotelManager.GetBookedInfoFlighPartAsync(request);

            var des = JsonConvert.SerializeObject(flightPart);

            if (request.GroupByStartFlightId)
            {
                var groupedFlightPart = await _bookedFlightAndHotelManager.GetBookedInfoFlighPartGroupAsync(flightPart);
                return Ok(groupedFlightPart);
            }
            else
            {
                return Ok(flightPart);
            }
        }
    }
}
