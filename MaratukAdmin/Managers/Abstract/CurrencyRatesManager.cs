using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Abstract
{
    public class CurrencyRatesManager : ICurrencyRatesManager
    {
        private readonly IMainRepository<CurrencyRates> _mainRepository;
        private readonly IMapper _mapper;

        public CurrencyRatesManager(IMainRepository<CurrencyRates> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }
        public async Task<CurrencyRates> AddCurrencyRatesAsync(AddCurrencyRates currency)
        {
            var entity = _mapper.Map<CurrencyRates>(currency);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteCurrencyRateByIdAsync(int id)
        {
            return await _mainRepository.DeleteAsync(id);
        }

        public  async Task<CurrencyRates> GetCurrencyRateNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<List<CurrencyRates>> GetCurrencyRatesAsync()
        {
            var result = await _mainRepository.GetAllAsync();
            return result.OrderBy(n => n.StartDate).ToList();
        }
    }
}
