using Bogus;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using static MaratukAdmin.Repositories.Concrete.Sansejour.HotelRepository;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{

    public class HotelRepository : IHotelRepository
    {
        protected readonly MaratukDbContext _dbContext;
        private readonly ICountryRepository _countryRepository;
        private readonly IHotelImagesRepository _hotelImagesRepository;
        private Faker faker = new();

        //private List<Hotel> Hotels = null;
        private List<MaratukAdmin.Entities.Global.Country> Countries = new();

        public HotelRepository(MaratukDbContext dbContext, ICountryRepository countryRepository, IHotelImagesRepository hotelImagesRepository)
        {
            _dbContext = dbContext;
            _countryRepository = countryRepository;
            _hotelImagesRepository = hotelImagesRepository;
        }

        public async Task EraseHotelListAsync()
        {
            //var tableName = _dbContext.Model.FindEntityType(typeof(Hotel)).GetTableName();
            //var truncateSql = $"TRUNCATE TABLE {tableName}";
            //_dbContext.Database.ExecuteSqlRaw(truncateSql);

            try
            {
                var allRecords = _dbContext.Hotel.ToList();
                _dbContext.Hotel.RemoveRange(allRecords);

                _dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Hotel', RESEED, 0)");

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _dbContext.Hotel.ToListAsync();
        }

        public async Task FillNewHotelsListAsync(List<Hotel> hotelList)
        {
            await _dbContext.Hotel.AddRangeAsync(hotelList);

            await _dbContext.SaveChangesAsync();
        }


        //public async Task<HotelResponseModel?> GetHoteByCodeAsync(string code)
        public async Task<HotelResponseModel?> GetHoteByCodeAsync(string code)
        {
            HotelResponseModel? retValue = new();
            //var query = from hot in _dbContext.Hotel
            //            where hot.Code == code
            //            join hotImages in _dbContext.HotelImages on hot.Id equals hotImages.HotelId //into hotImagesGroup
            //                                                                                        //from hotelImage in hotImagesGroup.DefaultIfEmpty()
            //            select new
            //            {
            //                //hotel = new Hotel()
            //                //{
            //                //    Address = hot.Address,
            //                //    City = hot.City,
            //                //    Code = hot.Code,
            //                //    Country = hot.Country,
            //                //    Description = hot.Description,
            //                //    Email = hot.Email,
            //                //    Fax = hot.Fax,
            //                //    GpsLatitude = hot.GpsLatitude,
            //                //    GpsLongitude = hot.GpsLongitude,
            //                //    HotelCategoryId = hot.HotelCategoryId,
            //                //    Id = hot.Id,
            //                //    IsCruise = hot.IsCruise,
            //                //    Name = hot.Name,
            //                //    PhoneNumber = hot.PhoneNumber,
            //                //    Site = hot.Site
            //                //},
            //                hotel = hot,
            //                //hotelImages = hotImagesGroup.ToList()
            //                hotelImages = hotImages
            //            };

            ////return await query.SingleOrDefaultAsync();

            //retValue = (HotelResponseModel)query;
            //return await Task.FromResult(retValue);

            var hot = _dbContext.Hotel.FirstOrDefaultAsync(h => h.Code == code).Result;

            if (hot != null)
            {
                retValue.hotel = hot;
                //var hotImages = _dbContext.HotelImages.Where(hi => hi.HotelId == hot.Id).ToList();
                var hotImages = _hotelImagesRepository.GetHotelImagesByHotelIdAsync(hot.Id).Result;

                retValue.hotelImages = hotImages;
            }

            return await Task.FromResult(retValue);
        }
        public async Task<HotelResponseModel> GetHoteByCodeMockAsync(string code)
        {
            HotelResponseModel retValue = GenerateFakeHotel(code);

            return await Task.FromResult(retValue);
        }

        private HotelResponseModel GenerateFakeHotel(string code)
        {
            HotelResponseModel fakeHotel;
            //Hotels = GetAllHotelsAsync().Result;
            Countries = _countryRepository.GetAllAsync().Result;
            try
            {
                int countryId;
                if (Countries == null)
                {
                    countryId = faker.Random.Number(1, 100);
                }
                else
                {
                    int countryIndex = faker.Random.Number(1, Countries.Count);
                    countryId = Countries[countryIndex].Id;
                }

                // Generate fake info
                fakeHotel = new HotelResponseModel()
                {
                    hotel = new Hotel()
                    {
                        Id = faker.Random.Number(1, 100),
                        Code = code,
                        Name = faker.Lorem.Word(),
                        //Country = Faker.RandomNumber.Next(1, 5000),
                        Country = countryId,
                        City = faker.Random.Number(1, 100),
                        HotelCategoryId = faker.Random.Number(1, 5),
                        //IsCruise = faker.Random.Number(0, 1),
                        IsCruise = faker.Random.Byte(0,1),

                        Address = faker.Address.FullAddress(),
                        GpsLatitude = faker.Address.Latitude().ToString(),
                        GpsLongitude = faker.Address.Longitude().ToString(),
                        PhoneNumber = faker.Phone.PhoneNumber(),
                        Fax = faker.Phone.PhoneNumber(),
                        Email = faker.Internet.Email(),
                        Site = "www." + faker.Lorem.Word() + ".com",
                        Description = faker.Lorem.Sentence(10)
                    }
                };

                // Generate fake hotel images
                var hotImages = _hotelImagesRepository.GetHotelImagesByHotelIdMockAsync(fakeHotel.hotel.Id).Result;

                fakeHotel.hotelImages = hotImages;
            }
            catch (Exception)
            {
                throw;
            }
            return fakeHotel;
        }
    }


}
