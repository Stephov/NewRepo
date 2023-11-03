using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IHotelManager
    {
        //Task<List<HotelResponse>> GetAllHotelsAsync();
        Task<List<Hotel>> GetAllHotelsAsync();
        
        //Task<HotelResponse> GetHotelByIdAsync(int id);
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<bool> RefreshHotelList();

    }
}
