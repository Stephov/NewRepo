using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{
    public class SyncSejourHotelRepository<T> : ISyncSejourHotelRepository<T> where T : SyncSejourHotel, new()
    //public class SyncSejourHotelRepository<T> : IMainRepository<T> where T : BaseDbEntity, new()
    {
        protected readonly MaratukDbContext _context;
        private readonly DbSet<T> _entities;

        public SyncSejourHotelRepository(MaratukDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        public virtual async Task<bool> DeleteSyncSejourHotelsByDateRAWAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                //string sqlQuery = @$"DELETE FROM {nameof(T)} 
                //                    WHERE {nameof(T.SyncDate)} = '{exportDate}'
                //                    AND {nameof(T.HotelCode)} = " + ((hotelCode == null)
                //                                                                ? nameof(T.HotelCode)
                //                                                                : "'" + hotelCode + "'");
                string sqlQuery = @$"DELETE FROM {nameof(SyncSejourHotel)} 
                                    WHERE {nameof(SyncSejourHotel.SyncDate)} = '{exportDate}'
                                    AND {nameof(SyncSejourHotel.HotelCode)} = " + ((hotelCode == null)
                                                                                ? nameof(SyncSejourHotel.HotelCode)
                                                                                : "'" + hotelCode + "'");
                await _context.Database.ExecuteSqlRawAsync(sqlQuery);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
