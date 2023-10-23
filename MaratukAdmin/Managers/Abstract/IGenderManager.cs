using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IGenderManager
    {
        Task<List<Gender>> GetGenderAsync();
        Task<Gender> GetGenderNameByIdAsync(int id);
    }
}
