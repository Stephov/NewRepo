using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IAirportRepository
    {
        Task<Airport?> GetAirportNameByCodeAsync(string code);
    }
}
