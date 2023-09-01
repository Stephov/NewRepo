using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class CurrencyManager : ICurrencyManager
    {

        private readonly IMainRepository<Currency> _mainRepository;
        private readonly IMapper _mapper;


        public CurrencyManager(IMainRepository<Currency> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<Currency> AddCurrencyAsync(AddCurrency currency)
        {
            var entity = _mapper.Map<Currency>(currency);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<Currency> GetCurrencyNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<List<Currency>> GetCurrencyAsync()
        {
            var result = await _mainRepository.GetAllAsync();
            return result.OrderBy(n => n.Name).ToList();
        }

        public async Task<bool> DeleteCurrencyByIdAsync(int id)
        {
            return await _mainRepository.DeleteAsync(id);
        }

        public async Task<Currency> UpdateCurrencyAsync(UpdateCurrency currency)
        {
            var entity = _mapper.Map<Currency>(currency);

            await _mainRepository.UpdateAsync(entity);
            return entity;
        }
    }
}
