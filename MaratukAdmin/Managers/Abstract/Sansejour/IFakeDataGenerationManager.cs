using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IFakeDataGenerationManager
    {
        List<RoomSearchResponse> GenerateFakeRooms(SearchFligtAndRoomRequest searchFligtAndRoomRequest, bool notRepeatableHotel);
        string GenerateHotelCode(bool notRepeatableHotel);
        List<HotelImage> GenerateFakeHotelImages(int hotelId);
        HotelResponseModel GenerateFakeHotel(string code);

    }
}
