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
        Task<HotelResponseModel> GetHotelByCodeMockAsync(string code);
        Task<List<Hotel>?> GetHotelsByCountryIdAndCityIdAsync(List<int>? countryIds = null, List<int>? cityIds = null);
        Task<List<HotelResponseModel>?> GetHotelsByCountryIdAndCityIdAsync(int? countryId = null, int? cityId = null);
        Task<Hotel> AddHotelAsync(AddHotelRequest hotelImageRequest);
        Task<Hotel> UpdateHotelAsync(UpdateHotelRequest hotel);
        Task<bool> RefreshHotelList();

    }
}
