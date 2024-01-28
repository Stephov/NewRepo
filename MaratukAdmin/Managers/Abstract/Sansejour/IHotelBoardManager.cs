using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IHotelBoardManager
    {
        Task<List<HotelBoard>> GetAllHotelBoardsAsync();
        Task<HotelBoard> GetHotelBoardByIdAsync(int id);
        Task<HotelBoard> GetHotelBoardByCodeAsync(string code);
        Task<HotelBoard> AddHotelBoardAsync(AddHotelBoardRequest hotelBoard);
        Task<HotelBoard> UpdateHotelBoardAsync(UpdateHotelBoardRequest hotelBoard);
    }
}
