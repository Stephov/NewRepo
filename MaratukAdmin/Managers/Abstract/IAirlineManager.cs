using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IAirlineManager
    {
        Task<List<Airline>> GetAirlinesAsync();
        Task<Airline> AddAirlineAsync(AddAirline airline);
        Task<Airline> GetAirlineNameByIdAsync(int id);
    }
}
