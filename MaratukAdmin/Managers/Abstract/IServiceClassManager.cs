using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IServiceClassManager
    {
        Task<List<ServiceClass>> GetServiceClassAsync();
        Task<ServiceClass> AddServiceClassAsync(AddServiceClass serviceClass);
        Task<ServiceClass> GetServiceClassNameByIdAsync(int id);
    }
}
