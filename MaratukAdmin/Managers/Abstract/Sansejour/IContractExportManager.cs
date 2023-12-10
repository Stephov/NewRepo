using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Managers.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IContractExportManager
    {
        Task<bool> GetSejourContractExportView(List<HotelSansejourResponse>? hotelsList = null);
        //Task<IActionResult> FillSejourContractExportView();

        Task<(int, int)> DescribeAccomodationTypes(List<SyncSejourAccomodationType> accmdType);
        Task<List<SyncSejourRate>> SearchRoomOldAsync(SearchRoomRequest searchRequest);
        Task<List<SyncSejourRate>> SearchRoomAsync(SearchRoomRequest searchRequest);
        Task<List<SyncSejourRate>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest);
        Task<List<SearchFligtAndRoomResponse>> SearchFlightAndRoomAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest);
        Task<List<SearchFligtAndRoomResponse>> SearchFlightAndRoomMockAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest);
        Task<List<SearchFligtAndRoomResponse>> SearchFlightAndRoomLowestPricesAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest);
        Task<List<SearchFligtAndRoomResponse>> SearchFlightAndRoomLowestPricesMockAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest);

    }
}
