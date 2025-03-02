﻿using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface ICurrencyManager
    {
        Task<List<Currency>> GetCurrencyAsync();
        Task<Currency> AddCurrencyAsync(AddCurrency currency);
        Task<Currency> GetCurrencyNameByIdAsync(int id);
        Task<bool> DeleteCurrencyByIdAsync(int id);
        Task<Currency> UpdateCurrencyAsync(UpdateCurrency currency);
    }
}
