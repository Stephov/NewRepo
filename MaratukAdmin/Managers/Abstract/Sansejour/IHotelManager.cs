using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using System.Threading.Tasks;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IHotelManager
    {
        //Task<List<HotelResponse>> GetAllHotelsAsync();
        Task<List<Hotel>> GetAllHotelsAsync();
        
        //Task<HotelResponse> GetHotelByIdAsync(int id);
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<HotelResponseModel> GetHotelByCodeAsync(string code);
        //Task<HotelResponseModel> GetHotelsByCountryAndCityNameAsync(List<string>? countryNames, List<string>? cityNames);
        Task<List<HotelResponseModel>?> GetHotelsByCountryIdAndCityIdAsync(bool includeImages, int? countryId = null, int? cityId = null);
        Task<List<HotelResponseModel>?> GetHotelsByCountryIdListAndCityIdListAsync(GetHotelsByCountryAndCityListRequest request);
        Task<List<Hotel>?> GetHotelsByCountryIdAndCityIdAsync(List<int>? countryIds = null, List<int>? cityIds = null);
        Task<HotelResponseModel> GetHotelByCodeMockAsync(string code);
        Task<Hotel> AddHotelAsync(AddHotelRequest hotelImageRequest);
        Task<Hotel> UpdateHotelAsync(UpdateHotelRequest hotel);
        Task<bool> RefreshHotelList();

    }
}
