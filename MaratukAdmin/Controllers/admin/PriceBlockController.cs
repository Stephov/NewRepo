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
    // [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class PriceBlockController : BaseController
    {

        private readonly IPriceBlockManager _priceBlockManager;
        public PriceBlockController(IPriceBlockManager priceBlockManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _priceBlockManager = priceBlockManager;
        }


        [HttpGet]
        public async Task<ActionResult> GetPriceBlocks()
        {
            var result = await _priceBlockManager.GetAllPriceBlockAsync();

            return Ok(result);
        }


/*        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetFlightById(int id)
        {
            var result = await _flightManager.GetFlightByIdAsync(id);

            return Ok(result);
        }*/


 /*       [HttpGet("info/{id:int}")]
        public async Task<ActionResult> GetFlightIifoById(int id)
        {
            var result = await _flightManager.GetFlightInfoByIdAsync(id);

            return Ok(result);
        }*/



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CreatePriceBlockAsync([FromBody] AddPriceBlockRequest priceBlockRequest)
        {
            try
            {
                //call manager
                var result = await _priceBlockManager.AddPriceBlockAsync(priceBlockRequest);

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
        public async Task<IActionResult> UpdateFlightAsync([FromBody] UpdatePriceBlockRequest priceBlockRequest)
        {
            var result = await _priceBlockManager.UpdatePriceBlockAsync(priceBlockRequest);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFlightAsync(int id)
        {
            var result = await _priceBlockManager.DeletePriceBlockAsync(id);
            return Ok(result);
        }


    }
}
