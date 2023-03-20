using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IFlightManager
    {
        Task<List<Flight>> GetAllFlightAsync();
        Task<Flight> GetFlightByIdAsync(int id);
        Task<Flight> AddFlightAsync(AddFlightRequest flight);
        Task<Flight> UpdateFlightAsync(UpdateFlightRequest flight);
        Task<bool> DeleteFlightAsync(int id);
    }
}
