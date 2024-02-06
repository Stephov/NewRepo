using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    //public interface IContractExportRepository<T> where T : BaseDbEntity, new()
    public interface IContractExportRepository
    {
        Task<List<SyncSejourContractExportView>> GetSyncSejourContractsByDateAsync(DateTime exportDate);
        Task<bool> DeleteSyncSejourContractsByDateAsync(DateTime exportDate);
        Task<bool> DeleteSyncedDataByDateAsync(DateTime exportDate);
        Task<bool> DeleteSyncedDataBySyncDateAndHotelCodeAsync(DateTime? exportDate, string hotelCode);
        Task<bool> DeleteSyncedDataByHotelCodeExceptSyncDateAsync(DateTime exportDate, string hotelCode);
        Task<bool> DeleteSyncedDataByHotelCodeAsync(DateTime exportDate, string hotelCode);


        Task AddlNewSejourContractsAsync(SyncSejourContractExportView contract);


        Task<List<SyncSejourHotel>> GetSyncSejourHotelsByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourHotelsByDateAsync(DateTime exportDate, string? hotelCode);
        Task<bool> DeleteSyncSejourHotelsByDateRAWAsync(DateTime? exportDate, string? hotelCode = null);
        Task AddNewSejourHotelsAsync(SyncSejourHotel syncHotel);


        Task<List<SyncSejourSpoAppOrder>> GetSyncSejourSpoAppOrdersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpoAppOrdersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpoAppOrdersByDateRAWAsync(DateTime? exportDate, string? hotelCode = null);
        Task AddNewSejourSpoAppOrdersAsync(List<SyncSejourSpoAppOrder> syncSpoAppOrders);


        Task<List<SyncSejourSpecialOffer>> GetSyncSejourSpecialOffersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpecialOffersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpecialOffersByDateRAWAsync(DateTime? exportDate, string? hotelCode = null);
        Task AddNewSejourSpecialOffersAsync(List<SyncSejourSpecialOffer> syncSpecialOffer);


        Task<SyncSejourRate> GetSyncSejourRateByIdAsync (int id);
        Task<List<SyncSejourRate>> GetSyncSejourRatesByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> GetSyncSejourRateExistenceByDateAndHotelAsync(string hotelCode, DateTime? exportDate = null);
        Task<bool> DeleteSyncSejourRatesByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourRatesByDateRAWAsync(DateTime? exportDate, string? hotelCode = null);
        Task AddNewSejourRatesAsync(List<SyncSejourRate> syncRate);


        Task<List<SyncSejourAccomodationType>> GetSyncSejourAccomodationTypeByCodeAsync(string? code = null);
        Task<HashSet<string>> GetHashSyncSejourAccomodationTypeByCodeAsync(string? code = null);
        Task<bool> DeleteSyncSejourAccomodationTypeByCodeAsync(string code );
        Task<bool> DeleteSyncSejourAccomodationTypeByCodeRAWAsync(string code);
        Task AddNewSejourChildAgesAsync(List<SyncSejourAccomodationType> syncChildAges);
        Task AddNewSejourChildAgesAsync(SyncSejourAccomodationType syncChildAges);

        Task<List<SyncSejourAccomodationType>> GetSyncSejourAccomodationTypesFromRatesBySyncDateAsync(DateTime syncDate);


        Task AddNewSyncSejourAccomodationTypesAsync(List<SyncSejourAccomodationType> accmdTypes);
        Task<(int, int)> DescribeListOfAccomodationTypesAsync(List<SyncSejourAccomodationType> accmdType);

        Task AddNewSyncSejourAccomodationDescriptionsAsync(List<SyncSejourAccomodationDescription> accmdList);
        Task<List<SyncSejourAccomodationDescription>> GetSyncSejourAccomodationDescriptionAsync(string? code = null);
        List<SyncSejourAccomodationDescription> DescribeAccomodationType(string accmdCode);


        Task<HashSet<string>> GetHashHotelBoardByCodeAsync(string? code = null);
        Task<List<HotelBoard>> GetHotelBoardsFromRatesBySyncDateAsync(DateTime syncDate);
        Task AddNewHotelBoardsAsync(List<HotelBoard> hotelBoards);
        Task<List<HotelBoard>> GetHotelBoardsAsync(string? code = null);
        Task<bool> DeleteHotelBoardByCodeAsync(string? code = null);


        Task<List<SyncSejourRate>> SearchRoomOldAsync(SearchRoomRequest searchRequest);
        //Task<List<SyncSejourRate>> SearchRoomAsync(SearchRoomRequest searchRequest);
        Task<List<RoomSearchResponse>> SearchRoomAsync(SearchRoomRequest searchRequest);
        //Task<List<SyncSejourRate>> SearchRoomMockAsync(SearchFligtAndRoomRequest searchFligtAndRoomRequest);
        Task<List<RoomSearchResponse>> SearchRoomMockAsync(SearchFligtAndRoomRequest searchFligtAndRoomRequest);
        Task<List<RoomSearchResponseLowestPrices>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest);
        //Task<List<SyncSejourRate>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest);
        //Task<List<SyncSejourRate>> SearchRoomLowestPricesMockAsync(SearchFligtAndRoomRequest searchFligtAndRoomRequest);
        Task<List<RoomSearchResponse>> SearchRoomLowestPricesMockAsync(SearchFligtAndRoomRequest searchFligtAndRoomRequest);
        Task<bool> UpdateSyncSejourRateSyncDateAsync (DateTime newSyncDate);
        Task<bool> UpdateSyncSejourRateSyncDateRAWAsync (DateTime newSyncDate);
        Task<bool> ArchiveSyncSejourRateData (DateTime newSyncDate);
        Task<DateTime?> GetMaxSyncDateAsync();
        Task<DateTime?> GetMaxSyncDateFromSejourRateAsync();
        
        DateTime? GetMaxSyncDate();
    }
}
