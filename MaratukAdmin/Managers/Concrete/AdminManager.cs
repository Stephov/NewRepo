using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Services;

namespace MaratukAdmin.Managers.Concrete
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepository _adminRepository;
        //test


        public AdminManager(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<City> GetCityByContryId(int countryId)
        {
            return await _adminRepository.GetCityByCountryIdAsync(countryId);
        }


        public async Task<List<Airline>> GetAllAirlinesAsync()
        {
            return await _adminRepository.GetAllAirlinesAsync();
        }

        public async Task<List<Aircraft>> GetAllAircraftsAsync()
        {
            return await _adminRepository.GetAllAircraftsAsync();
        }
    }
}
