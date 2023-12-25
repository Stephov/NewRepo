using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IHotelImagesManager
    {
        Task<List<HotelImage>> GetAllHotelImagesAsync();
        Task<HotelImage> GetHotelImageByImageIdAsync(int id);
        Task<List<HotelImage>> GetHotelImagesByHotelIdAsync(int hotelId);
        Task<List<HotelImage>> GetHotelImagesByHotelIdMockAsync(int hotelId);
        Task<List<HotelImage>?> GetHotelImagesByHotelCodeAsync(string hotelCode);
        //Task<HotelImageResponseModel> GetHotelImageByHotelCodeMockAsync(string code);
        //Task<HotelImage> UpdateHotelImageAsync(UpdateHotelImageRequest hotelImage);

        Task<HotelImage> AddHotelImageAsync(AddHotelImageRequest hotelImageRequest);
        Task<HotelImage> UpdateHotelImageAsync(UpdateHotelImageRequest hotelImageRequest);

    }
}
