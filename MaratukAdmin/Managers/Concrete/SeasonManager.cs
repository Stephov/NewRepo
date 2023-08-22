using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class SeasonManager : ISeasonManager
    {

        private readonly IMainRepository<Season> _mainRepository;
        private readonly IMapper _mapper;


        public SeasonManager(IMainRepository<Season> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<Season> AddSeasonAsync(AddSeason season)
        {
            var entity = _mapper.Map<Season>(season);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<Season> GetSeasonNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<List<Season>> GetSeasonAsync()
        {
            var result = await _mainRepository.GetAllAsync();
            return result.OrderBy(season => season.Name).ToList();
        }
    }
}
