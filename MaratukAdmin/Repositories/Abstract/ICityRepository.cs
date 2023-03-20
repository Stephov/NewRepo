using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface ICityRepository : IMainRepository<City>
    {
        Task<List<City>> GetCityByCountryIdAsync(int countryId);
    }
}
