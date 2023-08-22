using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class PricePackageController : BaseController
    {

        private readonly IPricePackageManager _pricePackageManager;
        public PricePackageController(IPricePackageManager pricePackageManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _pricePackageManager = pricePackageManager;
        }

        [HttpPost]
        public async Task<ActionResult> AddPricePackage(AddPricePackage addPricePackage)
        {
            var result = await _pricePackageManager.AddPricePackageAsync(addPricePackage);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetPricePackage()
        {
            var result = await _pricePackageManager.GetAllPricePackagesAsync();

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetPricePackage(int id)
        {
            var result = await _pricePackageManager.GetPricePackageByIdAsync(id);

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePricePackageAsync([FromBody] UpdatePricePackage pricePackage)
        {
            var result = await _pricePackageManager.UpdatePricePackageAsync(pricePackage);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            bool result = await _pricePackageManager.DeletePricePackageAsync(id);
            return Ok(result);
        }


    }
}
