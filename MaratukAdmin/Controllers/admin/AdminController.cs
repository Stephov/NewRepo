using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class AdminController : BaseController
    {
        private readonly ICountryManager _countryManager;
        private readonly ICityManager _cityManager;
        private readonly IAirlineManager _airlineManager;
        private readonly IAircraftManager _aircraftManager;
        private readonly IAirportManager _airportManager;
        private readonly IAirServiceManager _airServiceManager;
        public AdminController(ICountryManager countryManager,
                                 ICityManager cityManager,
                                 IAirlineManager airlineManager,
                                 IAircraftManager aircraftManager,
                                 IAirServiceManager airServiceManager,
                                 IAirportManager airportManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _countryManager = countryManager;
            _airlineManager = airlineManager;
            _cityManager = cityManager;
            _aircraftManager = aircraftManager;
            _airportManager = airportManager;
            _airServiceManager = airServiceManager;
        }


        [HttpGet("country")]
        public async Task<ActionResult> GetCountry()
        {
            var result = await _countryManager.GetAllCountryesAsync();

            return Ok(result);
        }



        [HttpGet("city")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCity(int countryId)
        {
            var result = await _cityManager.GetCityByCountryIdAsync(countryId);
 
            return Ok(result);
        }

        [HttpGet("Airline")]
        public async Task<ActionResult> GetAirline()
        {
            var result = await _airlineManager.GetAirlinesAsync();

            return Ok(result);
        }

        [HttpPost("Airline")]
        public async Task<ActionResult> AddPricePackage(AddAirline addAirline)
        {
            var result = await _airlineManager.AddAirlineAsync(addAirline);

            return Ok(result);
        }

        [HttpGet("Aircraft")]
        public async Task<ActionResult> GetAircraft()
        {
            var result = await _aircraftManager.GetAircraftsAsync();

            return Ok(result);
        }

        [HttpPost("Aircraft")]
        public async Task<ActionResult> AddAircraft(AddAircraft aircraft)
        {
            var result = await _aircraftManager.AddAircraftAsync(aircraft);

            return Ok(result);
        }

        [HttpPost("Airport")]
        public async Task<ActionResult> AddAirport(AddAirport airport)
        {
            var result = await _airportManager.AddAirportAsync(airport);

            return Ok(result);
        }

        [HttpGet("AirService")]
        public async Task<ActionResult> GetAirService()
        {
            var result = await _airServiceManager.GetAirServicesAsync();

            return Ok(result);
        }

    }
}
