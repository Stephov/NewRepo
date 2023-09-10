using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IFlightManager
    {
        Task<List<FlightResponse>> GetAllFlightAsync();
        //Task<CalendarSchedule> GetFlightCalendarInfoAsync(int flightId);
        Task<FlightEditResponse> GetFlightByIdAsync(int id);

        Task<List<FlightNameResponse>> GetFlightByIdsAsync(int departureCountryId, int departureCityId, int DestinationCountryId, int destinationCityId);

        Task<Flight> AddFlightAsync(AddFlightRequest flight);
        Task<Flight> UpdateFlightAsync(UpdateFlightRequest flight);
        Task<bool> DeleteFlightAsync(int id);
        Task<FlightInfoResponse> GetFlightInfoByIdAsync(int id);
        Task<bool> IsFlightNameExistAsync(string name);
    }
}
