using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IAircraftManager
    {
        Task<List<Aircraft>> GetAircraftsAsync();
        Task<Aircraft> AddAircraftAsync(AddAircraft airline);
    }
}
