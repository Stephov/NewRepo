using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using System.Collections.Generic;
using MaratukAdmin.Dto.Request.Sansejour;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class HotelImagesManager : IHotelImagesManager
    {
        private readonly IMainRepository<HotelImage> _mainRepository;
        private readonly IHotelImagesRepository _hotelImagesRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IHttpRequestManager _httpRequestManager;
        private readonly ITransactionRepository _transactionRepository;


        public HotelImagesManager(IMainRepository<HotelImage> mainRepository,
                            IHotelImagesRepository hotelImagesRepository,
                            IHotelRepository hotelRepository,
                            IHttpRequestManager httpRequestManager,
                            ITransactionRepository transactionRepository,
                            IDistributedCache cache

            )
        {
            _mainRepository = mainRepository;
            _hotelImagesRepository = hotelImagesRepository;
            _hotelRepository = hotelRepository;
            _httpRequestManager = httpRequestManager;
            _transactionRepository = transactionRepository;
        }


        public async Task<List<HotelImage>> GetAllHotelImagesAsync()
        {
            var result = await _mainRepository.GetAllAsync();

            return result;
        }


        public async Task<HotelImage> GetHotelImageByImageIdAsync(int imageId)
        {
            var result = await _mainRepository.GetAsync(imageId);

            return result;
        }

        public async Task<List<HotelImage>> GetHotelImagesByHotelIdAsync(int hotelId)
        {
            //var hotelImageEntity = await _hotelImagesRepository.GetHotelImagesByHotelIdAsync(hotelId);
            var hotelEntity = await _hotelRepository.GetHotelByIdAsync(hotelId);

            if (hotelEntity == null || hotelEntity.hotelImages == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return hotelEntity.hotelImages;
        }


        public async Task<List<HotelImage>?> GetHotelImagesByHotelCodeAsync(string hotelCode)
        {
            List<HotelImage>? hotelImages;

            var hotelEntity = await _hotelRepository.GetHotelByCodeAsync(hotelCode);
            if (hotelEntity == null)
            { throw new ApiBaseException(StatusCodes.Status404NotFound); }

            hotelImages = hotelEntity.hotelImages;

            return hotelImages;
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

        public async Task<HotelImage> UpdateHotelImageAsync(UpdateHotelImageRequest hotelImageRequest)
        {
            HotelImage result;
            IFormFile? fileContent = hotelImageRequest.FileContent;
            string? fileName = fileContent?.FileName;
            string? mediaType = fileContent?.ContentType;
            int? fileTypeId = hotelImageRequest.hotelImage.FileTypeId;

            try
            {
                HotelImage hotelImageEntity = await _mainRepository.GetAsync(hotelImageRequest.Id);
                if (hotelImageEntity == null)
                {
                    throw new ApiBaseException(StatusCodes.Status404NotFound);
                }

                //hotelImageEntity = hotelImageRequest.hotelImage;

                using var memoryStream = new MemoryStream();
                if (fileContent != null)
                { await fileContent.CopyToAsync(memoryStream); }
                byte[] fileBytes = memoryStream.ToArray();

                hotelImageEntity.FileName = fileName;
                hotelImageEntity.FileTypeId = fileTypeId;
                hotelImageEntity.MediaType = mediaType;
                hotelImageEntity.FileData = fileBytes;
                hotelImageEntity.HotelId = hotelImageRequest.hotelImage.HotelId;

                result = await _mainRepository.UpdateAsync(hotelImageEntity);

            }
            catch (Exception)
            {
                result = new();
            }

            return result;
            //return await _hotelImagesRepository.UpdateHotelImageAsync(hotelImageRequest);
        }


        public async Task<HotelImage> AddHotelImageAsync(AddHotelImageRequest hotelImageRequest)
        {
            return await _hotelImagesRepository.AddHotelImageAsync(hotelImageRequest);
        }
    }
}
