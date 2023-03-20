using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IAdminRepository
    {
        Task<List<Airline>> GetAllAirlinesAsync();
        Task<List<Aircraft>> GetAllAircraftsAsync();
        Task<City> GetCityByCountryIdAsync(int countryId);
    }
}
