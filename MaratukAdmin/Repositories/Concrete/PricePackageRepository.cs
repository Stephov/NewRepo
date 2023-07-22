using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class PricePackageRepository : MainRepository<PricePackage>, IPricePackageRepository
    {
        public PricePackageRepository(MaratukDbContext context) : base(context)
        {
        }

        public async Task<PricePaskageCountry> GetPricePaskageCountryAsync(int pricePackageId)
        {

            return await _context.PricePackage
            .Include(p => p.Country)
            .Where(p => p.Id == pricePackageId)
            .Select(group => new PricePaskageCountry
            {
                CountryId = group.CountryId,
                CountryName = group.Country.NameENG
            })
            .FirstOrDefaultAsync();

        }
    }
}
