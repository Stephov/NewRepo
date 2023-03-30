using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ICountryManager
    {
        Task<List<Country>> GetAllCountryesAsync();
        Task<Country> GetCountryNameByIdAsync(int id);
    }
}
