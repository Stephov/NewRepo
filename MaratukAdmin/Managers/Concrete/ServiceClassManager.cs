using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class ServiceClassManager : IServiceClassManager
    {

        private readonly IMainRepository<ServiceClass> _mainRepository;
        private readonly IMapper _mapper;


        public ServiceClassManager(IMainRepository<ServiceClass> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<ServiceClass> AddServiceClassAsync(AddServiceClass serviceClass)
        {
            var entity = _mapper.Map<ServiceClass>(serviceClass);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<ServiceClass> GetServiceClassNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<List<ServiceClass>> GetServiceClassAsync()
        {
            var result = await _mainRepository.GetAllAsync();
            return result.OrderBy(service => service.Name).ToList();
        }
    }
}
