using MaratukAdmin.Entities.Global;
using MaratukAdmin.Models;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IPricePackageRepository : IMainRepository<PricePackage>
    {
        Task<PricePaskageCountry> GetPricePaskageCountryAsync(int pricePackageId);
    }
}
