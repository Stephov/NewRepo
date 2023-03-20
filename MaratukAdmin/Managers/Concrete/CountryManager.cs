using AutoMapper;
using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class CountryManager : ICountryManager
    {
        private readonly IMainRepository<Country> _mainRepository;


        public CountryManager(IMainRepository<Country> mainRepository)
        {
            _mainRepository = mainRepository;
        }
        public async Task<List<Country>> GetAllCountryesAsync()
        {
           return await _mainRepository.GetAllAsync();
        }
    }
}
