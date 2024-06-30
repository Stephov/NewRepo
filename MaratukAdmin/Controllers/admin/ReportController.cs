﻿using MaratukAdmin.Entities.Report;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Mvc;
using static MaratukAdmin.Utils.Enums;

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
        public async Task<List<ReportFlightInfo>> GetReportFlightInfo(enumFlightReportType reportType, string? flightNumber = null)
        {
            var result = await _reportManager.GetReportFlightInfo(reportType, flightNumber);

            //return Ok(result);
            return result;
        }


        [HttpGet("GetTouristInfo")]
        //public async Task<List<ReportTouristInfoHotel>> GetTouristInfo(enumTouristReportType reportType, int priceBlockId)
        //public async Task<IActionResult> GetTouristInfo(enumTouristReportType reportType, int priceBlockId)
        public async Task<IActionResult> GetTouristInfo(enumTouristReportType reportType, DateTime? orderDateFrom = null, DateTime? orderDateTo = null, bool includeRate = false)
        {
            //if (reportType == enumTouristReportType.Flight)
            if (Enum.IsDefined(typeof(enumTouristReportType), reportType))
            {
                //var result = await _reportManager.GetReportTouristInfoAsync<ReportTouristInfoFlight> (reportType, priceBlockId);
                var result = await _reportManager.GetReportTouristInfoAsync<ReportTouristInfoFlight>(reportType, orderDateFrom, orderDateTo, includeRate);
                return Ok(result);
            }
            else
            {
                return BadRequest("Invalid report type");
            }

            //return Ok(result);
        }

        [HttpGet("GetAgencyDebts")]
        public async Task<IActionResult> GetAgencyDebtsInfo(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var result = await _reportManager.GetReportAgencyDebtsAsync<ReportAgencyDebts>(dateFrom, dateTo);
            return Ok(result);

        }
        
        [HttpGet("GetSalesByManagers")]
        public async Task<IActionResult> GetSalesByManagersInfo(DateTime? orderDateFrom = null, DateTime? orderDateTo = null, enumBookStatusForMaratuk bookStatus = enumBookStatusForMaratuk.All)
        {
            var result = await _reportManager.GetSalesByManagersAsync<ReportSalesByManagerPreparedData>(orderDateFrom, orderDateTo, bookStatus);
            return Ok(result);

        }
        
        [HttpGet("GetReportTotal")]
        public async Task<IActionResult> GetReportTotalInfo(DateTime? orderDateFrom = null, DateTime? orderDateTo = null, enumBookStatusForMaratuk bookStatus = enumBookStatusForMaratuk.All)
        {
            var result = await _reportManager.GetReportTotalAsync<ReportTotalPreparedData>(orderDateFrom, orderDateTo, bookStatus);
            return Ok(result);

        }
    }
}
