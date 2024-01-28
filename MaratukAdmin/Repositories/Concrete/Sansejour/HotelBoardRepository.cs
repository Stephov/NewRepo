using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{
    public class HotelBoardRepository : IHotelBoardRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public HotelBoardRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task EraseHotelBoardListAsync()
        {
            try
            {
                var allRecords = _dbContext.HotelBoard.ToList();
                _dbContext.HotelBoard.RemoveRange(allRecords);

                _dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Hotel', RESEED, 0)");

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task FillNewHotelsListAsync(List<HotelBoard> hotelBoardList)
        {
            await _dbContext.HotelBoard.AddRangeAsync(hotelBoardList);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<HotelBoard>> GetAllHotelBoardsAsync()
        {
            return await _dbContext.HotelBoard.ToListAsync();
        }

        public async Task<HotelBoard?> GetHotelBoardByCodeAsync(string code)
        {
            return await _dbContext.HotelBoard.FirstOrDefaultAsync(h => h.Board == code);
        }

        public async Task<HotelBoard?> GetHotelBoardByIdAsync(int id)
        {
            return await _dbContext.HotelBoard.FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
