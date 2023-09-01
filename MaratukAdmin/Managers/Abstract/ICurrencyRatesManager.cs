﻿using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ICurrencyRatesManager
    {
        Task<List<CurrencyRates>> GetCurrencyRatesAsync();
        Task<CurrencyRates> AddCurrencyRatesAsync(AddCurrencyRates currency);
        Task<CurrencyRates> GetCurrencyRateNameByIdAsync(int id);
        Task<bool> DeleteCurrencyRateByIdAsync(int id);
        Task<CurrencyRates> UpdateCurrencyRatesAsync(UpdateCurrencyRates currencyRates);
    }
}
