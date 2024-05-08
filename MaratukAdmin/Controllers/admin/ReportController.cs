using MaratukAdmin.Entities.Report;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : BaseController
    {
        private readonly IReportManager _reportManager;
        public ReportController(IReportManager reportManager,
                                JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _reportManager = reportManager;
        }


        [HttpGet("GetReportFlightInfo")]
        //public async Task<IActionResult> GetReportFlightInfo()
        //public async Task<List<ReportFlightInfo>> GetReportFlightInfo()
        public async Task<List<ReportFlightInfo>> GetReportFlightInfo()
        {
            var result = await _reportManager.GetReportFlightInfo();

            //return Ok(result);
            return result;
        }
    }
}
