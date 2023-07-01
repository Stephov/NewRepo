using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IPartnerManager
    {
        Task<List<Partner>> GetPartnerAsync();
        Task<Partner> AddPartnerAsync(AddPartner tarif);
        Task<Partner> GetPartnerNameByIdAsync(int id);
    }
}
