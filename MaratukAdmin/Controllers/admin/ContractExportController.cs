using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Managers.Concrete.Sansejour;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class ContractExportController : BaseController
    {
        private readonly IContractExportManager _contractExportManager;

        public ContractExportController(IContractExportManager contractExportManager,
                                        JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _contractExportManager = contractExportManager;
        }

        [HttpGet("GetSejourContractExportView/")]
        public async Task<IActionResult> GetSejourContractExportView()
        {
            var result = await _contractExportManager.GetSejourContractExportView();

            return Ok(result);
        }

        [HttpPost("SearchRoom/")]
        public async Task<IActionResult> SearchRoom([FromBody] SearchRoomRequest searchRequest)
        {
            //var result = await _contractExportManager.SearchRoomAsync(searchRequest);
            var result = await _contractExportManager.SearchRoomAsync(searchRequest);

            //var result = await _contractExportManager.SearchRoomAsync(searchRequest);

            return Ok(result);
        }

        [HttpPost("SearchRoomLowestPrices/")]
        public async Task<IActionResult> SearchRoomLowestPrices([FromBody] SearchRoomRequest searchRequest)
        {
            var result = await _contractExportManager.SearchRoomLowestPricesAsync(searchRequest);

            return Ok(result);
        }
    }
}
