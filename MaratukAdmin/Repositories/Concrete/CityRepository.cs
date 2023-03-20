using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class CityRepository : MainRepository<City>, ICityRepository
    {
        public CityRepository(MaratukDbContext context) : base(context)
        {
        }

        public async Task<List<City>> GetCityByCountryIdAsync(int countryId)
        {
   
            return await _context.City
            .Where(c => c.CountryId == countryId)
            .Include(p => p.Airports)
            .ToListAsync();

        }
    }
}
