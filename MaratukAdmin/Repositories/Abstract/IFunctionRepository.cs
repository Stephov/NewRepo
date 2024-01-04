using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IFunctionRepository
    {
        Task<List<FlightInfoFunction>> GetFligthInfoFunctionAsync(int TripTypeId);
        Task<List<SearchResultFunction>> GetFligthOneWayInfoFunctionAsync(int FlightId, DateTime FlightDate);
        Task<List<SearchResultFunctionTwoWay>> GetFligthTwoWayInfoFunctionAsync(int FlightOneWayId, int? FlightReturnedId, DateTime FlightStartDate,DateTime? FlightEndDate);
        Task<List<FinalFlightSearchResponse>> GetFligthInfoFunctionMockAsync(SearchFlightResult searchFlightResult);
        Task<List<FlightReturnDateForManual>> GetFlightReturnDateForManualAsync(DateTime Date,int FlightId);
        Task<List<FlightReturnDate>> GetFlightReturnDateAsync(int PriceBlockId,int DepartureCountryId ,int DepartureCityId ,int DestinationCountryId ,int DestinationCityId );
    }
}
