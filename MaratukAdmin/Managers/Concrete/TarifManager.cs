using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class TarifManager : ITarifManager
    {

        private readonly IMainRepository<Tarif> _mainRepository;
        private readonly IMapper _mapper;


        public TarifManager(IMainRepository<Tarif> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<Tarif> AddTarifAsync(AddTarif tarif)
        {
            var entity = _mapper.Map<Tarif>(tarif);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<Tarif> GetTarifNameByIdAsync(int id)
        {
           return await _mainRepository.GetAsync(id);
        }

        public async Task<List<Tarif>> GetTarifAsync()
        {
            return await _mainRepository.GetAllAsync();
        }
    }
}
