using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MaratukDbContext _dbContext;

        public AdminRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<City> GetCityByCountryIdAsync(int countryId)
        {
            return await _dbContext.City.AsNoTracking()
                     .Include(p => p.Airports)
                     .FirstOrDefaultAsync(u => u.CountryId == countryId);
        }

 

        public async Task<List<Airline>> GetAllAirlinesAsync()
        {
            return await _dbContext.Airline.ToListAsync();
        }

        public async Task<List<Aircraft>> GetAllAircraftsAsync()
        {
            return await _dbContext.Aircraft.ToListAsync();
        }
    }
}
