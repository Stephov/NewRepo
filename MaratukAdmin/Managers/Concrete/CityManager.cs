using MaratukAdmin.Entities.Global;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.Extensions.Hosting;

namespace MaratukAdmin.Managers.Concrete
{
    public class CityManager : ICityManager
    {

        private readonly ICityRepository _cityRepository;
        private readonly IMainRepository<City> _mainRepository;


        public CityManager(ICityRepository cityRepository, IMainRepository<City> mainRepository)
        {
            _cityRepository = cityRepository;
            _mainRepository = mainRepository;
        }

        public async Task<List<City>> GetCityByCountryIdAsync(int countryId)
        {
            var result = await _cityRepository.GetCityByCountryIdAsync(countryId);
            
            if(result == null)
            {
                throw new ArgumentException("Email or password is wrong");
            }

            return result;
        }

        public async Task<City> GetCityNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }
    }
}
