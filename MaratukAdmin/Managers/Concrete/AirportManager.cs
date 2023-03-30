using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class AirportManager : IAirportManager
    {

        private readonly IMainRepository<Airport> _mainRepository;
        private readonly IMapper _mapper;


        public AirportManager(IMainRepository<Airport> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }



        public async Task<Airport> AddAirportAsync(AddAirport aircraft)
        {
            var entity = _mapper.Map<Airport>(aircraft);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<Airport> GetAirportNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }
    }
}
