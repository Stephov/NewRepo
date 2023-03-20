using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ICityManager
    {
        Task<List<City>> GetCityByCountryIdAsync(int countryId);
    }
}
