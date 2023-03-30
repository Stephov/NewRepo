using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class AircraftManager : IAircraftManager
    {

        private readonly IMainRepository<Aircraft> _mainRepository;
        private readonly IMapper _mapper;


        public AircraftManager(IMainRepository<Aircraft> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<List<Aircraft>> GetAircraftsAsync()
        {
           return await _mainRepository.GetAllAsync();
        }


        public async Task<Aircraft> AddAircraftAsync(AddAircraft aircraft)
        {
            var entity = _mapper.Map<Aircraft>(aircraft);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<Aircraft> GetAircraftNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }
    }
}
