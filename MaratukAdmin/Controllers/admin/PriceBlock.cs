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

        private readonly IPricePackageManager _pricePackageManager;
        public PriceBlockController(IPricePackageManager pricePackageManager,
                               JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _pricePackageManager = pricePackageManager;
        }
        [HttpGet]
        public async Task<ActionResult> GetPriceBlock()
        {
            var result = "hello";

            return Ok(result);
        }


    }
}
