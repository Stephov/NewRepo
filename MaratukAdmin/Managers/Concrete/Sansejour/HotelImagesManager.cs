using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.Extensions.Caching.Distributed;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class HotelImagesManager : IHotelImagesManager
    {
        private readonly IMainRepository<HotelImage> _mainRepository;
        private readonly IHotelImagesRepository _hotelImagesRepository;
        private readonly IHttpRequestManager _httpRequestManager;
        private readonly ITransactionRepository _transactionRepository;

        public HotelImagesManager(IMainRepository<HotelImage> mainRepository,
                            IHotelImagesRepository hotelImagesRepository,
                            IHttpRequestManager httpRequestManager,
                            ITransactionRepository transactionRepository,
                            IDistributedCache cache

            )
        {
            _mainRepository = mainRepository;
            _hotelImagesRepository = hotelImagesRepository;
            _httpRequestManager = httpRequestManager;
            _transactionRepository = transactionRepository;
        }


        public async Task<List<HotelImage>> GetAllHotelImagesAsync()
        {
            var result = await _mainRepository.GetAllAsync();

            return result;
        }

        public async Task<List<HotelImage>> GetHotelImagesByHotelIdAsync(int hotelId)
        {

            var entity = await _mainRepository.GetAllAsync(hotelId.ToString());

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;
        }

        public async Task<List<HotelImage>> GetHotelImagesByHotelIdMockAsync(int hotelId)
        {
            var entity = await _hotelImagesRepository.GetHotelImagesByHotelIdMockAsync(hotelId);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;

        }
    }
}
