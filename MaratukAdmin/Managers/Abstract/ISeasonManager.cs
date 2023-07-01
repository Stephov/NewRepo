using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ISeasonManager
    {
        Task<List<Season>> GetSeasonAsync();
        Task<Season> AddSeasonAsync(AddSeason season);
        Task<Season> GetSeasonNameByIdAsync(int id);
    }
}
