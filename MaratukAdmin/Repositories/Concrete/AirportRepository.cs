using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class AirportRepository: IAirportRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public AirportRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Airport?> GetAirportNameByCodeAsync(string code)
        {
            var result = await _dbContext.Airport
                                   .Where(c => c.Code == code)
                                   .FirstOrDefaultAsync();
            return result;
        }
    }
}
