using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;

namespace MaratukAdmin.Managers.Concrete
{
    public class AirServiceManager : IAirServiceManager
    {

        private readonly IAirServiceRepository _airServiceRepository;


        public AirServiceManager(IAirServiceRepository airServiceRepository)
        {
            _airServiceRepository = airServiceRepository;
        }

        public async Task<List<AirService>> GetAirServicesAsync()
        {
            var result = await _airServiceRepository.GetAllAsync();
            return result.OrderBy(n => n.Name).ToList();
        }
    }
}
