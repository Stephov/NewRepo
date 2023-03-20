using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IAirServiceManager
    {
        Task<List<AirService>> GetAirServicesAsync();
    }
}
