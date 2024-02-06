using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IHotelRepository
    {
        //Task<Hotel> GetHotelByIdAsync(int id);
        Task<HotelResponseModel?> GetHotelByCodeAsync(string code);
        Task<HotelResponseModel?> GetHotelByIdAsync(int id);
        Task<HotelResponseModel> GetHoteByCodeMockAsync(string code);
        Task<List<Hotel>> GetAllHotelsAsync();
        Task<List<Hotel>?> GetHotelsByCountryIdAndCityIdAsync(List<int>? countryIds = null, List<int>? cityIds = null);
        Task EraseHotelListAsync();
        Task FillNewHotelsListAsync(List<Hotel> hotelList);
        //Task<Hotel> AddHotelAsync(AddHotelRequest hotelRequest);

    }
}
