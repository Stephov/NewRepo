using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IAirportManager
    {
        Task<Airport> AddAirportAsync(AddAirport airport);
        Task<Airport> GetAirportNameByIdAsync(int id);
    }
}
