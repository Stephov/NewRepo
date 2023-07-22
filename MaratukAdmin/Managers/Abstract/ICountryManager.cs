using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Models;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ICountryManager
    {
        Task<List<Country>> GetAllCountryesAsync();
        Task<Country> GetCountryNameByIdAsync(int id);
        Task<List<FlightCountryResponse>> GetDistinctCountriesAndCities();
    }
}
