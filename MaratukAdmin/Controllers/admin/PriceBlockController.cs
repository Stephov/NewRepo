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
            try
            {
                var result = await _priceBlockManager.GetAllPriceBlockAsync();
                return Ok(result);

            }
            catch (Exception ex)
            {
                var res = ex.Message;
            }

            return Ok();
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetPriceBlockId(int id)
        {
            var result = await _priceBlockManager.GetPriceBlockByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("PriceBlockServices/{id:int}")]
        public async Task<ActionResult> GetPriceBlockServiceByProceBlockId(int id)
        {
            var result = await _priceBlockManager.GetServicesByPriceBlockIdAsync(id);

            return Ok(result);
        }




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

        [HttpPost("ServicesPricingPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CreateServicesPricingPolicyAsync([FromBody] AddServicesPricingPolicy addServicesPricingPolicy)
        {
            try
            {
                //call manager
                var result = await _priceBlockManager.CreateServicesPricingPolicyAsync(addServicesPricingPolicy);

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


        [HttpPut("ServicesPricingPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateServicesPricingPolicyAsync([FromBody] EditServicesPricingPolicy updateServicesPricingPolicy)
        {
            try
            {
                //call manager
                var result = await _priceBlockManager.UpdateServicesPricingPolicyAsync(updateServicesPricingPolicy);

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


        [HttpPost("PriceBlockService")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CreatePriceBlockServiceAsync([FromBody] AddPriceBlockServicesRequest priceBlockServiceRequest)
        {
            try
            {
                //call manager
                var result = await _priceBlockManager.AddPriceBlockServicesAsync(priceBlockServiceRequest);

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
        public async Task<IActionResult> UpdatePriceBlockAsync([FromBody] UpdatePriceBlockRequest priceBlockRequest)
        {
            var result = await _priceBlockManager.UpdatePriceBlockAsync(priceBlockRequest);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePriceBlockAsync(int id)
        {
            var result = await _priceBlockManager.DeletePriceBlockAsync(id);
            return Ok(result);
        }

        [HttpDelete("PriceBlockService/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePriceBlockServiceAsync(int id)
        {
            var result = await _priceBlockManager.DeletePriceBlockServiceAsync(id);
            return Ok(result);
        }


        [HttpDelete("ServicesPricingPolicy/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteServicesPricingPolicyeAsync(int id)
        {
            var result = await _priceBlockManager.DeleteServicesPricingPolicyAsync(id);
            return Ok(result);
        }

        [HttpGet("ServicesPricingPolicy/{id:int}")]
        public async Task<ActionResult> GetServicesPricingPolicy(int id)
        {
            var result = await _priceBlockManager.GetServicesPricingPolicyByPriceBlockServicesIdAsync(id);

            return Ok(result);
        }

    }
}
