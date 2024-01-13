using Bogus;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Managers.Concrete.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{
    public class HotelImagesRepository : IHotelImagesRepository
    {
        protected readonly MaratukDbContext _dbContext;
        //private readonly IMainRepository<HotelImage> _mainRepository;
        private Faker faker = new();
        private readonly string filePath = @"\\16.171.6.213\C$\Uploads\";
        protected readonly IFakeDataGenerationManager _fakeDataGenerationManager;

        public HotelImagesRepository(MaratukDbContext dbContext, IFakeDataGenerationManager fakeDataGenerationManager)
        {
            _dbContext = dbContext;
            _fakeDataGenerationManager = fakeDataGenerationManager;
        }

        public async Task<HotelImage> AddHotelImageAsync(AddHotelImageRequest hotelImageRequest)
        {
            //string fullPath = filePath + fileContent.FileName;
            IFormFile fileContent = hotelImageRequest.FileContent;
            string fileName = fileContent.FileName;
            string mediaType = fileContent.ContentType;

            try
            {
                //using (var stream = new FileStream(fullPath, FileMode.Create))
                //{
                //    await fileContent.CopyToAsync(stream);
                //}

                using var memoryStream = new MemoryStream();
                await fileContent.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                HotelImage newHotelImage = new()
                {
                    FileName = fileName,
                    //FilePath = filePath,
                    FileTypeId = hotelImageRequest.FileTypeId,
                    MediaType = mediaType,
                    FileData = fileBytes,
                    HotelId = hotelImageRequest.HotelId,
                };

                await _dbContext.HotelImage.AddAsync(newHotelImage);

                await _dbContext.SaveChangesAsync();

                return newHotelImage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HotelImage> UpdateHotelImageAsync(UpdateHotelImageRequest hotelImageRequest)
        {
            IFormFile? fileContent = hotelImageRequest.FileContent;
            string? fileName = fileContent?.FileName;
            string? mediaType = fileContent?.ContentType;
            int? fileTypeId = hotelImageRequest.hotelImage.FileTypeId;

            try
            {
                using var memoryStream = new MemoryStream();
                if (fileContent != null)
                { await fileContent.CopyToAsync(memoryStream); }
                byte[] fileBytes = memoryStream.ToArray();

                HotelImage newHotelImage = new()
                {
                    FileName = fileName,
                    //FilePath = filePath,
                    FileTypeId = fileTypeId,
                    MediaType = mediaType,
                    FileData = fileBytes,
                    HotelId = hotelImageRequest.hotelImage.HotelId,
                };

                //await _dbContext.HotelImage.AddAsync(newHotelImage);

                await _dbContext.SaveChangesAsync();

                return newHotelImage;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<HotelImage>> GetAllHotelImagesAsync()
        {
            return await _dbContext.HotelImage.ToListAsync();
        }

        public async Task<List<HotelImage>> GetAllHotelImagesMockAsync()
        {
            return await _dbContext.HotelImage.ToListAsync();
        }

        public async Task<List<HotelImage>> GetHotelImagesByHotelIdAsync(int hotelId)
        {
            List<HotelImage> retValue = await _dbContext.HotelImage.Where(hi => hi.HotelId == hotelId).ToListAsync();
            return retValue;
        }

        public async Task<List<HotelImage>> GetHotelImagesByHotelIdMockAsync(int hotelId)
        {
            return await Task.FromResult(_fakeDataGenerationManager.GenerateFakeHotelImages(hotelId));
        }
    }
}
