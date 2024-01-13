using Bogus;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
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
        private readonly IFakeDataGenerationManager _fakeDataGenerationManager;

        //private List<Hotel> Hotels = null;
        

        public HotelRepository(MaratukDbContext dbContext
                    , ICountryRepository countryRepository
                    , IHotelImagesRepository hotelImagesRepository
                    , IFakeDataGenerationManager fakeDataGenerationManager)
        {
            _dbContext = dbContext;
            _countryRepository = countryRepository;
            _hotelImagesRepository = hotelImagesRepository;
            _fakeDataGenerationManager = fakeDataGenerationManager;
        }


        //public async Task<Hotel> AddHotelAsync(AddHotelRequest hotelRequest)
        //{
        //    try
        //    {
        //        var entity = _mapper.Map<Hotel>(hotelRequest);
        //        await _mainRepository.AddAsync(entity);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

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
        public async Task<HotelResponseModel?> GetHotelByCodeAsync(string code)
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

        public async Task<HotelResponseModel?> GetHotelByIdAsync(int id)
        {
            HotelResponseModel? retValue = new();

            var hot = _dbContext.Hotel.FirstOrDefaultAsync(h => h.Id == id).Result;

            if (hot != null)
            {
                retValue.hotel = hot;
                var hotImages = _hotelImagesRepository.GetHotelImagesByHotelIdAsync(hot.Id).Result;

                retValue.hotelImages = hotImages;
            }

            return await Task.FromResult(retValue);
        }

        public async Task<HotelResponseModel> GetHoteByCodeMockAsync(string code)
        {
            HotelResponseModel retValue = _fakeDataGenerationManager.GenerateFakeHotel(code);

            return await Task.FromResult(retValue);
        }

    }


}
