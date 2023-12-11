using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IHotelRepository
    {
        //Task<Hotel> GetHotelByIdAsync(int id);
        Task<Hotel> GetHoteByCodeMockAsync(string code);
        Task<List<Hotel>> GetAllHotelsAsync();
        Task EraseHotelListAsync();
        Task FillNewHotelsListAsync(List<Hotel> hotelList);

    }
}
