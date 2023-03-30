using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IFlightManager
    {
        Task<List<FlightResponse>> GetAllFlightAsync();
        Task<FlightEditResponse> GetFlightByIdAsync(int id);
        Task<Flight> AddFlightAsync(AddFlightRequest flight);
        Task<Flight> UpdateFlightAsync(UpdateFlightRequest flight);
        Task<bool> DeleteFlightAsync(int id);
        Task<FlightInfoResponse> GetFlightInfoByIdAsync(int id);
    }
}
