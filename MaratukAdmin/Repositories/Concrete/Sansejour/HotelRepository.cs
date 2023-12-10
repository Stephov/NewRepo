using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly MaratukDbContext _dbContext;
        private readonly ICountryRepository _countryRepository;

        private List<Hotel> Hotels = null;
        private List<MaratukAdmin.Entities.Global.Country> Countries = null;

        public HotelRepository(MaratukDbContext dbContext, ICountryRepository countryRepository)
        {
            _dbContext = dbContext;
            _countryRepository = countryRepository;
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

        public async Task<Hotel> GetHoteByCodeMockAsync(string code)
        {
            Hotel retValue = GenerateFakeHotel(code);

            return await Task.FromResult(retValue);
        }

        private Hotel GenerateFakeHotel(string code)
        {
            Hotel fakeHotel;
            //Hotels = GetAllHotelsAsync().Result;
            Countries = _countryRepository.GetAllAsync().Result;
            try
            {
                int countryId;
                if (Countries == null)
                {
                    countryId = Faker.RandomNumber.Next(1, 100);
                }
                else
                {
                    int countryIndex = Faker.RandomNumber.Next(1, Countries.Count);
                    countryId = Countries[countryIndex].Id;
                }
                // Generate fake info
                fakeHotel = new Hotel()
                {
                    Id = Faker.RandomNumber.Next(1, 100),
                    Code = code,
                    Name = Faker.Lorem.Words(1).FirstOrDefault(),
                    //Country = Faker.RandomNumber.Next(1, 5000),
                    Country = countryId,
                    City = Faker.RandomNumber.Next(1, 100),
                    HotelCategoryId = Faker.RandomNumber.Next(1, 5),
                    IsCruise = Faker.RandomNumber.Next(0, 1),

                    Address = Faker.Lorem.Words(1).FirstOrDefault(),
                    GpsLatitude = Faker.StringFaker.Numeric(10),
                    GpsLongitude = Faker.StringFaker.Numeric(10),
                    PhoneNumber = Faker.Phone.Number(),
                    Fax = Faker.Phone.Number(), 
                    Email = Faker.Internet.Email(),
                    Site = "www." + Faker.Lorem.Words(1).FirstOrDefault() + ".com",
                    Description = Faker.Lorem.Sentence(10)
                };
            }
            catch (Exception)
            {
                throw;
            }
            return fakeHotel;
        }
    }
}
