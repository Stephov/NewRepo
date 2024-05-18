using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ICurrencyRatesManager
    {
        Task<List<CurrencyRatesResponse>> GetCurrencyRatesAsync();
        Task<List<CurrencyRatesResponse>> GetCurrencyRatesAsync(DateTime? date = null, string? rate = null);
        Task<CurrencyRates> AddCurrencyRatesAsync(AddCurrencyRates currency);
        Task<CurrencyRates> GetCurrencyRateNameByIdAsync(int id);
        Task<bool> DeleteCurrencyRateByIdAsync(int id);
        Task<CurrencyRates> UpdateCurrencyRatesAsync(UpdateCurrencyRates currencyRates);
    }
}
