using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IHotelBoardRepository
    {
        Task<HotelBoard?> GetHotelBoardByCodeAsync(string code);
        Task<HotelBoard?> GetHotelBoardByIdAsync(int id);
        Task<List<HotelBoard>> GetAllHotelBoardsAsync();
        Task EraseHotelBoardListAsync();
        Task FillNewHotelsListAsync(List<HotelBoard> hotelList);
    }
}
