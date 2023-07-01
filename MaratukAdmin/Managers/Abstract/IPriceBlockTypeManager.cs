using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IPriceBlockTypeManager
    {
        Task<List<PriceBlockType>> GetPriceBlockTypeAsync();
        Task<PriceBlockType> AddPriceBlockTypeAsync(AddPriceBlockType priceBlockType);
        Task<PriceBlockType> GetPriceBlockTypeNameByIdAsync(int id);
    }
}
