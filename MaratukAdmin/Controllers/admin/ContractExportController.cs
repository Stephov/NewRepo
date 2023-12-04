﻿using AutoMapper.Configuration.Conventions;
using MaratukAdmin.Dto.Request;
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

        [HttpPost("SearchFlightAndRoom/")]
        public async Task<IActionResult> SearchFlightAndRoom([FromBody] SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        {
            var result = await _contractExportManager.SearchFlightAndRoomAsync(searchFlightAndRoomRequest);

            return Ok(result);
        }
        
        [HttpPost("SearchFlightAndRoomLowestPrices/")]
        public async Task<IActionResult> SearchFlightAndRoomLowestPrices([FromBody] SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        {
            var result = await _contractExportManager.SearchFlightAndRoomLowestPricesAsync(searchFlightAndRoomRequest);

            return Ok(result);
        }


        //{
        //  "flightOneId": 286,
        //  "flightTwoId": 287,
        //  "startDate": "2023-11-28",
        //  "returnedDate": "2023-12-05",
        //  "adult": 1,
        //  "child": 1,
        //  "infant": 0
        //}
    }
}
