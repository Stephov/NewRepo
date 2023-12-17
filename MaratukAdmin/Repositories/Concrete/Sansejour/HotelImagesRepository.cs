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
        private string filePath = "C:/_IMAGES/";

        public HotelImagesRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HotelImage> AddHotelImageAsync(AddHotelImageRequest hotelImageRequest)
        {
            IFormFile file = hotelImageRequest.FileContent;
            string fullPath = filePath + file.FileName;

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                HotelImage newHotelImage = new()
                {
                    FileName = file.FileName,
                    FilePath = filePath,
                    FileTypeId = hotelImageRequest.FileTypeId,
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
