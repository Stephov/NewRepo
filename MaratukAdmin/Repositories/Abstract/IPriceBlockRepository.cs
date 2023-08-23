using MaratukAdmin.Entities;
using System.Threading.Tasks;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IPriceBlockRepository
    {
        Task<IEnumerable<PriceBlock>> GetAllPriceBlocksAsync();
        Task<PriceBlock> GetPriceBlockByIdAsync(int id);
        Task<IEnumerable<PriceBlockServices>> GetServicesByPriceBlockIdAsync(int id);
        Task<PriceBlock> CreatePriceBlockAsync(PriceBlock priceBlock);
        Task UpdatePriceBlockAsync(PriceBlock priceBlock);
        Task DeletePriceBlockAsync(int id);
        //Task<IEnumerable<PriceBlockServices>> GetPriceBlockServicesByPriceBlockIdAsync(int priceBlockId);
        Task<PriceBlockServices> CreatePriceBlockServicesAsync(PriceBlockServices priceBlockServices);


        /// ServicesPricingPolicy
        Task<ServicesPricingPolicy> CreateServicesPricingPolicyAsync(ServicesPricingPolicy priceBlockServices);
        Task<List<ServicesPricingPolicy>> GetServicesPricingPolicyByPriceBlockServicesIdAsync(int id);
        Task<bool> DeleteServicesPricingPolicyAsync(int id);
        Task<ServicesPricingPolicy> UpdateServicesPricingPolicyAsync(ServicesPricingPolicy priceBlock);
        Task<ServicesPricingPolicy> GetServicesPricingPolicyByIdAsync(int id);




        Task<bool> DeletePriceBlockServicesAsync(int id);
    }
}
