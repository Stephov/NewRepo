using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IHotelImagesManager
    {
        Task<List<HotelImage>> GetAllHotelImagesAsync();
        Task<List<HotelImage>> GetHotelImagesByHotelIdAsync(int hotelId);
        Task<List<HotelImage>> GetHotelImagesByHotelIdMockAsync(int hotelId);
    }
}
