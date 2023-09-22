using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IFunctionRepository
    {
        Task<List<FlightInfoFunction>> GetFligthInfoFunctionAsync(int TripTypeId);
        Task<List<FlightReturnDateForManual>> GetFlightReturnDateForManualAsync(DateTime Date,int FlightId);
        Task<List<FlightReturnDate>> GetFlightReturnDateAsync(int PriceBlockId,int DepartureCountryId ,int DepartureCityId ,int DestinationCountryId ,int DestinationCityId );
    }
}
