using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IPriceBlockManager
    {
        //Task<List<FlightResponse>> GetAllPriceBlockAsync();
        //Task<FlightEditResponse> GetPriceBlockByIdAsync(int id);
        Task<PriceBlock> AddPriceBlockAsync(AddPriceBlockRequest priceBlock);
        Task<PriceBlock> UpdatePriceBlockAsync(UpdatePriceBlockRequest priceBlock);
        Task<bool> DeletePriceBlockAsync(int id);
        //Task<FlightInfoResponse> GetPriceBlockInfoByIdAsync(int id);
    }
}
