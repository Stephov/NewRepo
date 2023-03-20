using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class AirlineManager : IAirlineManager
    {

        private readonly IMainRepository<Airline> _mainRepository;
        private readonly IMapper _mapper;


        public AirlineManager(IMainRepository<Airline> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<Airline> AddAirlineAsync(AddAirline airline)
        {
            var entity = _mapper.Map<Airline>(airline);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<List<Airline>> GetAirlinesAsync()
        {
            return await _mainRepository.GetAllAsync();
        }
    }
}
