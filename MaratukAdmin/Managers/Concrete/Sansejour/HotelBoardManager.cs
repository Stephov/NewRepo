using AutoMapper;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class HotelBoardManager : IHotelBoardManager
    {
        private readonly IMainRepository<HotelBoard> _mainRepository;
        private readonly IMapper _mapper;
        private readonly IHotelBoardRepository _hotelBoardRepository;
        private readonly IHttpRequestManager _httpRequestManager;
        private readonly ITransactionRepository _transactionRepository;
        //private readonly IDistributedCache _cache;

        public HotelBoardManager(IMainRepository<HotelBoard> mainRepository,
                            IMapper mapper,
                            IHotelBoardRepository hotelBoardRepository,
                            IHttpRequestManager httpRequestManager,
                            ITransactionRepository transactionRepository
                            //IDistributedCache cache

            )
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
            _hotelBoardRepository = hotelBoardRepository;
            _httpRequestManager = httpRequestManager;
            _transactionRepository = transactionRepository;
            //_cache = cache;
        }

        public async Task<List<HotelBoard>> GetAllHotelBoardsAsync()
        {
            //var result = await _mainRepository.GetAllAsync();
            //return result;

            return await _hotelBoardRepository.GetAllHotelBoardsAsync();
        }

        public async Task<HotelBoard> GetHotelBoardByCodeAsync(string code)
        {
            var entity = await _hotelBoardRepository.GetHotelBoardByCodeAsync(code);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;
        }

        public async Task<HotelBoard> GetHotelBoardByIdAsync(int id)
        {
            var entity = await _mainRepository.GetAsync(id);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;
        }

        public Task<HotelBoard> AddHotelBoardAsync(AddHotelBoardRequest hotelBoard)
        {
            throw new NotImplementedException();
        }

        public Task<HotelBoard> UpdateHotelBoardAsync(UpdateHotelBoardRequest hotelBoard)
        {
            throw new NotImplementedException();
        }
    }
}
