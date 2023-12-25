using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Repositories.Abstract.Sansejour
{
    public interface IHotelImagesRepository
    {
        Task<List<HotelImage>> GetHotelImagesByHotelIdAsync(int hotelId);
        Task<List<HotelImage>> GetHotelImagesByHotelIdMockAsync(int hotelId);
        Task<List<HotelImage>> GetAllHotelImagesAsync();
        Task<List<HotelImage>> GetAllHotelImagesMockAsync();
        
        Task<HotelImage> AddHotelImageAsync(AddHotelImageRequest hotelImageRequest);
        //Task<HotelImage> UpdateHotelImageAsync(UpdateHotelImageRequest hotelImageRequest);

    }
}
