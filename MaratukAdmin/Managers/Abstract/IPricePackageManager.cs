using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Models;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IPricePackageManager
    {
        Task<List<PricePackageResponse>> GetAllPricePackagesAsync();
        Task<PricePackage> GetPricePackageByIdAsync(int id);
        Task<PricePackage> AddPricePackageAsync(AddPricePackage pricePackage);
        Task<PricePackage> UpdatePricePackageAsync(UpdatePricePackage pricePackage);
        Task<bool> DeletePricePackageAsync(int id);
        Task<PricePaskageCountry> GetPricePaskageCountryAsync(int pricePackageId);
    }
}
