using Bogus;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
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

        public HotelImagesRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
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

                await _dbContext.HotelImages.AddAsync(newHotelImage);

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
            return await _dbContext.HotelImages.ToListAsync();
        }

        public async Task<List<HotelImage>> GetAllHotelImagesMockAsync()
        {
            return await _dbContext.HotelImages.ToListAsync();
        }

        public async Task<List<HotelImage>> GetHotelImagesByHotelIdAsync(int hotelId)
        {
            return await _dbContext.HotelImages.Where(hi => hi.HotelId == hotelId).ToListAsync();
        }

        public async Task<List<HotelImage>> GetHotelImagesByHotelIdMockAsync(int hotelId)
        {
            return await Task.FromResult(GenerateFakeHotelImages(hotelId));
        }

        private List<HotelImage> GenerateFakeHotelImages(int hotelId)
        {
            List<HotelImage> fakeHotelImages = new();

            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    HotelImage fakeHotelImage = new()
                    {
                        HotelId = hotelId,
                        FileTypeId = faker.Random.Number(1, 2),
                        FileName = faker.Random.Word() + ".jpg",
                        FilePath = faker.System.FilePath()
                    };

                    fakeHotelImages.Add(fakeHotelImage);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fakeHotelImages;
        }
    }
}
