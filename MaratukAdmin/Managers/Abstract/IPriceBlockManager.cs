﻿using MaratukAdmin.Dto.Request;
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

        Task<List<GroupedFlight>> GetSearchInfoAsync(int TripTypeId,bool isOnlyFligth);

        Task<DateResponse> GetFligthDateInfoAsync(int FlightId, int PriceBlockId, int DepartureCountryId, int DepartureCityId, int DestinationCountryId, int DestinationCityId, DateTime FromDate);
        Task<List<FinalFlightSearchResponse>> GetFligthSearchResultAsync(SearchFlightResult searchFlightResult);
        Task<List<FinalFlightSearchResponse>> GetFligthSearchResultMockAsync(SearchFlightResult searchFlightResult);

        Task<PriceBlockServices> AddPriceBlockServicesAsync(AddPriceBlockServicesRequest priceBlock);
        //Task<FlightInfoResponse> GetPriceBlockInfoByIdAsync(int id);
    }
}
