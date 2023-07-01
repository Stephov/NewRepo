using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IPriceBlockRepository
    {
        Task<IEnumerable<PriceBlock>> GetAllPriceBlocksAsync();
        Task<PriceBlock> GetPriceBlockByIdAsync(int id);
        Task<PriceBlock> CreatePriceBlockAsync(PriceBlock priceBlock);
        Task UpdatePriceBlockAsync(PriceBlock priceBlock);
        Task DeletePriceBlockAsync(int id);
        Task<IEnumerable<PriceBlockServices>> GetPriceBlockServicesByPriceBlockIdAsync(int priceBlockId);
        Task CreatePriceBlockServicesAsync(PriceBlockServices priceBlockServices);
        Task UpdatePriceBlockServicesAsync(PriceBlockServices priceBlockServices);
        Task DeletePriceBlockServicesAsync(int id);
    }
}
