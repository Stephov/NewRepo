using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IPriceBlockManager
    {
        Task<List<PriceBlockResponse>> GetAllPriceBlockAsync();
        Task<PriceBlockEditResponse> GetPriceBlockByIdAsync(int id);
        Task<List<PriceBlockServicesResponse>> GetServicesByPriceBlockIdAsync(int id);
        Task<PriceBlock> AddPriceBlockAsync(AddPriceBlockRequest priceBlock);
        Task<PriceBlock> UpdatePriceBlockAsync(UpdatePriceBlockRequest priceBlock);
        Task<bool> DeletePriceBlockAsync(int id);
        Task<bool> DeletePriceBlockServiceAsync(int id);


        Task<ServicesPricingPolicy> CreateServicesPricingPolicyAsync(AddServicesPricingPolicy priceBlockServices);
        Task<bool> DeleteServicesPricingPolicyAsync(int id);
        Task<List<ServicesPricingPolicy>> GetServicesPricingPolicyByPriceBlockServicesIdAsync(int id);
        Task<ServicesPricingPolicy> UpdateServicesPricingPolicyAsync(EditServicesPricingPolicy editServicesPricingPolicy);


        Task<PriceBlockServices> AddPriceBlockServicesAsync(AddPriceBlockServicesRequest priceBlock);
        //Task<FlightInfoResponse> GetPriceBlockInfoByIdAsync(int id);
    }
}
