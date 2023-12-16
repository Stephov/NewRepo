using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;

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
        Task<Hotel> UpdateHotelAsync(UpdateHotelRequest hotel);
        Task<bool> RefreshHotelList();

    }
}
