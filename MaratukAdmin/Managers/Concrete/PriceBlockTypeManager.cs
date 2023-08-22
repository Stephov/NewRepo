using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class PriceBlockTypeManager : IPriceBlockTypeManager
    {

        private readonly IMainRepository<PriceBlockType> _mainRepository;
        private readonly IMapper _mapper;


        public PriceBlockTypeManager(IMainRepository<PriceBlockType> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<PriceBlockType> AddPriceBlockTypeAsync(AddPriceBlockType priceBlockType)
        {
            var entity = _mapper.Map<PriceBlockType>(priceBlockType);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<PriceBlockType> GetPriceBlockTypeNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<List<PriceBlockType>> GetPriceBlockTypeAsync()
        {
            var result = await _mainRepository.GetAllAsync();
            return result.OrderBy(pbt => pbt.Name).ToList();
        }
    }
}
