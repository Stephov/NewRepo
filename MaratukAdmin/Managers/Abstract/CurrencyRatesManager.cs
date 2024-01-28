using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Abstract
{
    public class CurrencyRatesManager : ICurrencyRatesManager
    {
        private readonly IMainRepository<CurrencyRates> _mainRepository;
        private readonly IMainRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyRatesManager(IMainRepository<CurrencyRates> mainRepository, IMapper mapper, IMainRepository<Currency> currencyRepository)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
            _currencyRepository = currencyRepository;
        }
        public async Task<CurrencyRates> AddCurrencyRatesAsync(AddCurrencyRates currency)
        {
            var entity = _mapper.Map<CurrencyRates>(currency);
            entity.UpdateDate = DateTime.UtcNow;
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteCurrencyRateByIdAsync(int id)
        {
            return await _mainRepository.DeleteAsync(id);
        }

        public async Task<CurrencyRates> GetCurrencyRateNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<List<CurrencyRates>> GetCurrencyRatesAsync()
        {
            var result = await _mainRepository.GetAllAsync();


            List<CurrencyRates> lastCurrencies = result
    .GroupBy(c => c.CurrencyId)
    .Select(group => group.OrderByDescending(c => c.UpdateDate).FirstOrDefault())
    .ToList();


            return lastCurrencies;
        }

        public async Task<CurrencyRates> UpdateCurrencyRatesAsync(UpdateCurrencyRates currencyRates)
        {
            var entity = _mapper.Map<CurrencyRates>(currencyRates);
            entity.UpdateDate = DateTime.UtcNow;
            await _mainRepository.UpdateAsync(entity);
            return entity;
        }
    }
}
