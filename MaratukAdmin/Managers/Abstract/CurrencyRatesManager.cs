using AutoMapper;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Repositories.Abstract;
using Currency = MaratukAdmin.Entities.Global.Currency;

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

        public async Task<List<CurrencyRatesResponse>> GetCurrencyRatesAsync()
        {
            List<CurrencyRatesResponse> currencyRatesResponses = new List<CurrencyRatesResponse>();


            var result = await _mainRepository.GetAllAsync();

            List<CurrencyRates> lastCurrencies = result
                                                .GroupBy(c => c.CurrencyId)
                                                .Select(group => group.OrderByDescending(c => c.UpdateDate).FirstOrDefault())
                                                .ToList();

            foreach (var currencyRate in lastCurrencies)
            {
                CurrencyRatesResponse currencyRatesResponse = new CurrencyRatesResponse();
                currencyRatesResponse.CurrencyId = currencyRate.CurrencyId;
                currencyRatesResponse.Id = currencyRate.Id;
                currencyRatesResponse.CodeIso = _currencyRepository.GetAsync(currencyRate.CurrencyId).Result.CodeIso;
                currencyRatesResponse.OfficialRate = currencyRate.OfficialRate;
                currencyRatesResponse.InternaRate = currencyRate.InternaRate;
                currencyRatesResponse.UpdateDate = currencyRate.UpdateDate;

                currencyRatesResponses.Add(currencyRatesResponse);
            }
            

            return currencyRatesResponses;
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
