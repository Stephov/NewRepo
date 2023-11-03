using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task EraseHotelListAsync();
        Task FillNewHotelsListAsync(List<Hotel> hotelList);

    }
}
