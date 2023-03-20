using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IPricePackageManager
    {
        Task<List<PricePackage>> GetAllPricePackagesAsync();
        Task<PricePackage> GetPricePackageByIdAsync(int id);
        Task<PricePackage> AddPricePackageAsync(AddPricePackage pricePackage);
        Task<PricePackage> UpdatePricePackageAsync(UpdatePricePackage pricePackage);
        Task<bool> DeletePricePackageAsync(int id);
    }
}
