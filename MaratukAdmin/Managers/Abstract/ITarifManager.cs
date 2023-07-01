using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ITarifManager
    {
        Task<List<Tarif>> GetTarifAsync();
        Task<Tarif> AddTarifAsync(AddTarif tarif);
        Task<Tarif> GetTarifNameByIdAsync(int id);
    }
}
