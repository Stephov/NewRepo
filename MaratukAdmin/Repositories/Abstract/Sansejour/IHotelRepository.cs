using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IHotelRepository
    {
        //Task<Hotel> GetHotelByIdAsync(int id);
        Task<HotelResponseModel?> GetHoteByCodeAsync(string code);
        Task<HotelResponseModel> GetHoteByCodeMockAsync(string code);
        Task<List<Hotel>> GetAllHotelsAsync();
        Task EraseHotelListAsync();
        Task FillNewHotelsListAsync(List<Hotel> hotelList);

    }
}
