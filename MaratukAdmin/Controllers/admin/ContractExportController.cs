using AutoMapper.Configuration.Conventions;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Managers.Concrete.Sansejour;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        //public async Task<IActionResult> GetSejourContractExportView()
        public async Task<IActionResult> GetSejourContractExportView([FromQuery] string? hotelCode)
        {
            var result = await _contractExportManager.GetSejourContractExportView(hotelCode);

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
        public async Task<IActionResult> SearchFlightAndRoom([FromBody] SearchFligtAndRoomRequestBaseModel requestModel)
        {
            SearchFligtAndRoomRequest searchFlightAndRoomRequest = new(requestModel);
            //{
            //    FlightOneId = requestModel.FlightOneId,
            //    FlightTwoId = requestModel.FlightTwoId,
            //    FlightStartDate = requestModel.FlightStartDate,
            //    FlightReturnedDate = requestModel.FlightReturnedDate,
            //    RoomAdultCount = requestModel.RoomAdultCount,
            //    RoomChildCount = requestModel.RoomChildCount,
            //    RoomChildAges = requestModel.RoomChildAges
            //};

            var result = await _contractExportManager.SearchFlightAndRoomAsync(searchFlightAndRoomRequest);

            return Ok(result);
        }

        [HttpPost("SearchFlightAndRoomMock/")]
        //public async Task<IActionResult> SearchFlightAndRoomMockAsync([FromBody] SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        public async Task<IActionResult> SearchFlightAndRoomMockAsync([FromBody] SearchFligtAndRoomRequestBaseModel requestModel)
        {
            SearchFligtAndRoomRequest searchFlightAndRoomRequest = new(requestModel);
            //var result = await _contractExportManager.SearchFlightAndRoomMockAsync(searchFlightAndRoomRequest);
            var result = await _contractExportManager.SearchFlightAndRoomMockAsync(searchFlightAndRoomRequest);

            return Ok(result);
        }

        [HttpPost("SearchFlightAndRoomLowestPrices/")]
        //public async Task<IActionResult> SearchFlightAndRoomLowestPrices([FromBody] SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        public async Task<IActionResult> SearchFlightAndRoomLowestPrices([FromBody] SearchFligtAndRoomRequestBaseModel requestModel)
        {
            SearchFligtAndRoomRequest searchFlightAndRoomRequest = new(requestModel);

            //var result = await _contractExportManager.SearchFlightAndRoomLowestPricesAsync(searchFlightAndRoomRequest);
            var result = await _contractExportManager.SearchFlightAndRoomLowestPricesAsync(searchFlightAndRoomRequest);

            return Ok(result);
        }

        [HttpPost("SearchFlightAndRoomLowestPricesMock/")]
        //public async Task<IActionResult> SearchFlightAndRoomLowestPricesMock([FromBody] SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        public async Task<IActionResult> SearchFlightAndRoomLowestPricesMock([FromBody] SearchFligtAndRoomRequestBaseModel requestModel)
        {
            SearchFligtAndRoomRequest searchFlightAndRoomRequest = new(requestModel);

            //var result = await _contractExportManager.SearchFlightAndRoomLowestPricesMockAsync(searchFlightAndRoomRequest);
            var result = await _contractExportManager.SearchFlightAndRoomLowestPricesMockAsync(searchFlightAndRoomRequest);

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
