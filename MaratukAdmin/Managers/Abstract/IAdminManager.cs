using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IAdminManager
    {
        Task<List<Airline>> GetAllAirlinesAsync();
        Task<List<Aircraft>> GetAllAircraftsAsync();
        Task<City> GetCityByContryId(int countryId);
        Task<List<TripType>> GetTripTypesAsync();
    }
}
