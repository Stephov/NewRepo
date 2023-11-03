using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    //public interface IContractExportRepository<T> where T : BaseDbEntity, new()
    public interface IContractExportRepository
    {
        Task<List<SyncSejourContractExportView>> GetSyncSejourContractsByDateAsync(DateTime exportDate);
        Task<bool> DeleteSyncSejourContractsByDateAsync(DateTime exportDate);
        Task<bool> DeleteSyncedDataByDateAsync(DateTime exportDate);
        Task AddlNewSejourContractsAsync(SyncSejourContractExportView contract);


        Task<List<SyncSejourHotel>> GetSyncSejourHotelsByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourHotelsByDateAsync(DateTime exportDate, string? hotelCode);
        Task<bool> DeleteSyncSejourHotelsByDateRAWAsync(DateTime exportDate, string? hotelCode = null);
        Task AddNewSejourHotelsAsync(SyncSejourHotel syncHotel);


        Task<List<SyncSejourSpoAppOrder>> GetSyncSejourSpoAppOrdersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpoAppOrdersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpoAppOrdersByDateRAWAsync(DateTime exportDate, string? hotelCode = null);
        Task AddNewSejourSpoAppOrdersAsync(List<SyncSejourSpoAppOrder> syncSpoAppOrders);


        Task<List<SyncSejourSpecialOffer>> GetSyncSejourSpecialOffersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpecialOffersByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourSpecialOffersByDateRAWAsync(DateTime exportDate, string? hotelCode = null);
        Task AddNewSejourSpecialOffersAsync(List<SyncSejourSpecialOffer> syncSpecialOffer);


        Task<List<SyncSejourRate>> GetSyncSejourRatesByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourRatesByDateAsync(DateTime exportDate, string? hotelCode = null);
        Task<bool> DeleteSyncSejourRatesByDateRAWAsync(DateTime exportDate, string? hotelCode = null);
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


        Task<List<SyncSejourRate>> SearchRoomOldAsync(SearchRoomRequest searchRequest);
        Task<List<SyncSejourRate>> SearchRoomAsync(SearchRoomRequest searchRequest);
        Task<List<SyncSejourRate>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest);
        Task<DateTime?> GetMaxSyncDateAsync();
        DateTime? GetMaxSyncDate();
    }
}
