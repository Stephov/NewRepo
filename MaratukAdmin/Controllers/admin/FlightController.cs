﻿using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class FlightController : BaseController
    {

        private readonly IFlightManager _flightManager;
        public FlightController(IFlightManager flightManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _flightManager = flightManager;
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
            var result = await _flightManager.GetFlightByIdsAsync(departureCountryId,departureCityId,destinationCountryId,destinationCityId);

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

    }
}
