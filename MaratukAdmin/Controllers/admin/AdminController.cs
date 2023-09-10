using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class AdminController : BaseController
    {
        private readonly ICountryManager _countryManager;
        private readonly ICityManager _cityManager;
        private readonly IAdminManager _adminManager;
        private readonly IAirlineManager _airlineManager;
        private readonly IAircraftManager _aircraftManager;
        private readonly IAirportManager _airportManager;
        private readonly IAirServiceManager _airServiceManager;
        private readonly ITarifManager _tarifManager;
        private readonly IServiceClassManager _serviceClassManager;
        private readonly ISeasonManager _seasonManager;
        private readonly IPriceBlockTypeManager _priceBlockTypeManager;
        private readonly IPricePackageManager _pricePackageManager;
        private readonly IPartnerManager _partnerManager;
        private readonly ICurrencyManager _currencyManager;
        private readonly ICurrencyRatesManager _currencyRatesManager;
        private readonly IPriceBlockManager _priceBlockManager;
        public AdminController(ICountryManager countryManager,
                                 ICityManager cityManager,
                                 IAirlineManager airlineManager,
                                 IAircraftManager aircraftManager,
                                 IAirServiceManager airServiceManager,
                                 IAirportManager airportManager,
                                 ITarifManager tarifManager,
                                 IServiceClassManager serviceClassManager,
                                 ISeasonManager seasonManager,
                                 IPriceBlockTypeManager priceBlockTypeManager,
                                 IPricePackageManager pricePackageManager,
                                 IPartnerManager partnerManager,
                                 ICurrencyManager currencyManager,
                                 ICurrencyRatesManager currencyRatesManager,
                                 IAdminManager adminManager,
                                 IPriceBlockManager priceBlockManager,
        JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _countryManager = countryManager;
            _airlineManager = airlineManager;
            _cityManager = cityManager;
            _aircraftManager = aircraftManager;
            _airportManager = airportManager;
            _airServiceManager = airServiceManager;
            _tarifManager = tarifManager;
            _serviceClassManager = serviceClassManager;
            _seasonManager = seasonManager;
            _priceBlockTypeManager = priceBlockTypeManager;
            _pricePackageManager = pricePackageManager;
            _partnerManager = partnerManager;
            _currencyManager = currencyManager;
            _currencyRatesManager = currencyRatesManager;
            _adminManager = adminManager;
            _priceBlockManager = priceBlockManager;
        }


        [HttpGet("country")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCountry()
        {
            var result = await _countryManager.GetAllCountryesAsync();

            return Ok(result);
        }

        [HttpGet("countryFlight")]
        public async Task<ActionResult> GetCountryFlight()
        {
            var result = await _countryManager.GetDistinctCountriesAndCities();

            return Ok(result);
        }

        [HttpGet("PricePackageCountry")]
        public async Task<ActionResult> GetPricePackageCountry(int pricePackageId)
        {
            var result = await _pricePackageManager.GetPricePaskageCountryAsync(pricePackageId);

            return Ok(result);
        }

        [HttpGet("healthy")]
        public async Task<ActionResult> Healthy()
        {
            return Ok("healthy");
        }




        [HttpGet("city")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
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

        [HttpGet("Tarif")]
        public async Task<ActionResult> GetTarif()
        {
            var result = await _tarifManager.GetTarifAsync();

            return Ok(result);
        }


        [HttpGet("Tarif/{id:int}")]
        public async Task<ActionResult> GetTarifById(int id)
        {
            var result = await _tarifManager.GetTarifNameByIdAsync(id);

            return Ok(result);
        }


        [HttpGet("ServiceClass")]
        public async Task<ActionResult> GetServiceClass()
        {
            var result = await _serviceClassManager.GetServiceClassAsync();

            return Ok(result);
        }

        [HttpGet("ServiceClass/{id:int}")]
        public async Task<ActionResult> GetServiceClassById(int id)
        {
            var result = await _serviceClassManager.GetServiceClassNameByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("Season")]
        public async Task<ActionResult> GetSeason()
        {
            var result = await _seasonManager.GetSeasonAsync();

            return Ok(result);
        }

        [HttpGet("Season/{id:int}")]
        public async Task<ActionResult> GetSeasonById(int id)
        {
            var result = await _seasonManager.GetSeasonNameByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("PriceBlockType")]
        public async Task<ActionResult> GetPriceBlockType()
        {
            var result = await _priceBlockTypeManager.GetPriceBlockTypeAsync();

            return Ok(result);
        }

        [HttpGet("PriceBlockType/{id:int}")]
        public async Task<ActionResult> GetPriceBlockTypeById(int id)
        {
            var result = await _priceBlockTypeManager.GetPriceBlockTypeNameByIdAsync(id);

            return Ok(result);
        }


        [HttpGet("Partner")]
        public async Task<ActionResult> GetPartner()
        {
            var result = await _partnerManager.GetPartnerAsync();

            return Ok(result);
        }

        [HttpGet("Partner/{id:int}")]
        public async Task<ActionResult> GetPartnerTypeById(int id)
        {
            var result = await _partnerManager.GetPartnerNameByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("Currency")]
        public async Task<ActionResult> GetCurrency()
        {
            var result = await _currencyManager.GetCurrencyAsync();

            return Ok(result);
        }

        [HttpGet("CurrencyRates")]
        public async Task<ActionResult> GetCurrencyRates()
        {
            var result = await _currencyRatesManager.GetCurrencyRatesAsync();

            return Ok(result);
        }

        [HttpDelete("Currency")]
        public async Task<ActionResult> DeleteCurrency(int id)
        {
            var result = await _currencyManager.DeleteCurrencyByIdAsync(id);

            return Ok(result);
        }

        [HttpDelete("CurrencyRates")]
        public async Task<ActionResult> DeleteCurrencyRates(int id)
        {
            var result = await _currencyRatesManager.DeleteCurrencyRateByIdAsync(id);

            return Ok(result);
        }


        [HttpPost("Currency")]
        public async Task<ActionResult> GetCurrency(AddCurrency currency)
        {
            var result = await _currencyManager.AddCurrencyAsync(currency);

            return Ok(result);
        }



        [HttpPost("CurrencyRates")]
        public async Task<ActionResult> GetCurrencyRates(AddCurrencyRates currency)
        {
            var result = await _currencyRatesManager.AddCurrencyRatesAsync(currency);

            return Ok(result);
        }

        [HttpPut("CurrencyRates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCurrencyRatesAsync([FromBody] UpdateCurrencyRates currencyRates)
        {
            var result = await _currencyRatesManager.UpdateCurrencyRatesAsync(currencyRates);
            return Ok(result);
        }

        [HttpPut("Currency")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCurrencyAsync([FromBody] UpdateCurrency currency)
        {
            var result = await _currencyManager.UpdateCurrencyAsync(currency);
            return Ok(result);
        }

        [HttpGet("Currency/{id:int}")]
        public async Task<ActionResult> GetCurrencyById(int id)
        {
            var result = await _currencyManager.GetCurrencyNameByIdAsync(id);

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

        [HttpGet("TripType")]
        public async Task<ActionResult> GetTripType()
        {
            var result = await _adminManager.GetTripTypesAsync();

            return Ok(result);
        }

        [HttpGet("PriceBlockState")]
        public async Task<ActionResult> GetPriceBlockState()
        {
            var result = await _adminManager.PriceBlockStateAsync();

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

        [HttpGet("FligthCountryInfo")]
        public async Task<ActionResult> FligthCountryInfo(int TripTypeId)
        {
            var result = await _priceBlockManager.GetSearchInfoAsync(TripTypeId);

            return Ok(result);
        }
    }
}
