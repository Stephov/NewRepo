using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface ISyncSejourHotelRepository<T> where T : SyncSejourHotel, new()
    {
        Task<bool> DeleteSyncSejourHotelsByDateRAWAsync(DateTime exportDate, string? hotelCode = null);
    }
}
