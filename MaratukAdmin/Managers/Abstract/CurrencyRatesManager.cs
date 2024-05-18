using AutoMapper;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

        public async Task<List<CurrencyRatesResponse>> GetCurrencyRatesAsync(DateTime? date = null, string? currency = null)
        {
            currency = currency?.ToUpper() ?? null;
            //int? currencyId = _currencyRepository.GetAllAsync().Result.FirstOrDefault(c => c.CodeIso == currency).Id;
            var res = await _currencyRepository.GetAllAsync();
            var currencyRecord = res?.FirstOrDefault(c => c.CodeIso == currency);
            int? currencyId = currencyRecord?.Id;

            List<CurrencyRatesResponse> currencyRatesResponses = new List<CurrencyRatesResponse>();

            var result = await _mainRepository.GetAllAsync();

            // Got No Date, No currency param
            if (date == null && currency == null)
            {
                result = result
                        .GroupBy(r => r.CurrencyId)
                        .Select(g => g.OrderByDescending(r => r.UpdateDate).First())
                        .ToList();
            }
            // Got only Currency param
            else if (currency != null && date == null)
            {
                result = result
                    .Where(r => r.CurrencyId == currencyId)
                    .OrderByDescending(r => r.UpdateDate)
                    .Take(1)
                    .ToList();
            }
            // Got only Date param
            else if (date != null && currency == null)
            {
                result = result
                    .Where(r => r.UpdateDate.Date <= date)
                    .GroupBy(r => r.CurrencyId)
                    .Select(g => g.OrderByDescending(r => r.UpdateDate).First())
                    .ToList();
            }
            // Got Date AND Currency params
            else if (date != null && currency != null)
            {
                result = result
                    .Where(r => r.CurrencyId == currencyId && r.UpdateDate.Date <= date)
                    .OrderByDescending(r => r.UpdateDate)
                    .Take(1)
                    .ToList();

                // If no info found - Then search for latest rate for this currency
                if (result == null || result.Count == 0)
                {
                    return await GetCurrencyRatesAsync(default, currency);
                }
            }
            //List<CurrencyRates> lastCurrencies = result
            //                                    .GroupBy(c => c.CurrencyId)
            //                                    .Select(group => group.OrderByDescending(c => c.UpdateDate).FirstOrDefault())
            //                                    .ToList();

            //foreach (var currencyRate in lastCurrencies)
            foreach (var currencyRate in result)
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
