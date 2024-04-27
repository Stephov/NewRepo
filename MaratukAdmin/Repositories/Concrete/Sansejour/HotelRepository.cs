using Bogus;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
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
using System.Linq;
using static MaratukAdmin.Repositories.Concrete.Sansejour.HotelRepository;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
        public async Task<List<Hotel>?> GetHotelsByCountryIdAndCityIdAsync(List<int>? countryIds = null, List<int>? cityIds = null)
        {
            //return await _dbContext.Hotel.Where(h => h.Country == 
            //                                    ((countryIds == null) ? h.Country : countryIds) 
            //                                    && h.City == ((cityIds == null) ? h.City : cityIds)
            //                                    ).ToListAsync();

            return await _dbContext.Hotel.Where(h =>
                                                (countryIds == null || !countryIds.Any() || countryIds.Contains((int)h.Country))
                                                &&
                                                (cityIds == null || !cityIds.Any() || cityIds.Contains((int)h.City))
                                                ).ToListAsync();


            //var hotels = dbContext.Hotel
            //    .Where(hotel => hotel.Country == 6226 && cityIds.Contains(hotel.City));
        }

        public async Task<List<HotelResponseModel>?> GetHotelsByCountryIdAndCityIdAsync(bool includeImages, int? countryId = null, int? cityId = null)
        {
            //var bbb = await (from hotel in _dbContext.Hotel
            //                 where (countryId == null || (int)hotel.Country == countryId) &&
            //                       (cityId == null || (int)hotel.City == cityId)
            //                 select new HotelResponseModel
            //                 {
            //                     hotel = hotel,
            //                     hotelImages = _dbContext.HotelImage.Where(hi => hi.HotelId == hotel.Id).ToList()
            //                 }).ToListAsync();
            List<HotelResponseModel>? result = null;

            try
            {
                if (includeImages)
                {
                    result = await (from hotel in _dbContext.Hotel
                                    join ctry in _dbContext.Country
                                    on hotel.Country equals ctry.Id
                                    join cty in _dbContext.City
                                    on hotel.City equals cty.Id
                                    join hotelImages in _dbContext.HotelImage
                                    on hotel.Id equals hotelImages.HotelId into hotelImageGroup
                                    from hotelImages in hotelImageGroup.DefaultIfEmpty()
                                    where (countryId == null || (int)hotel.Country == countryId)
                                       && (cityId == null || (int)hotel.City == cityId)
                                    select new HotelResponseModel()
                                    {
                                        hotel = hotel,
                                        hotelCityName = cty.Name,
                                        hotelCityNameEng = cty.NameEng,
                                        hotelCountryName = ctry.Name,
                                        hotelCountryNameEng = ctry.NameENG,
                                        hotelImages = _dbContext.HotelImage.Where(hi => hi.HotelId == hotel.Id).ToList()
                                        //hotelImages = hotelImageGroup.Where(hi => hi.HotelId == hotel.Id).ToList()
                                    }).ToListAsync();
                }
                else
                {
                    result = await (from hotel in _dbContext.Hotel
                                    join ctry in _dbContext.Country
                                    on hotel.Country equals ctry.Id
                                    join cty in _dbContext.City
                                    on hotel.City equals cty.Id
                                    where (countryId == null || (int)hotel.Country == countryId)
                                       && (cityId == null || (int)hotel.City == cityId)
                                    select new HotelResponseModel()
                                    {
                                        hotel = hotel,
                                        hotelCityName = cty.Name,
                                        hotelCityNameEng = cty.NameEng,
                                        hotelCountryName = ctry.Name,
                                        hotelCountryNameEng = ctry.NameENG,
                                        hotelImages = null
                                    }).ToListAsync();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;

            //return await _dbContext.Hotel.Where(h =>
            //                                (countryId == null || (int)h.Country == countryId)
            //                                &&
            //                                (cityId == null || (int)h.City == cityId)
            //                                ).ToListAsync();
        }

        public async Task<List<HotelResponseModel>?> GetHotelsByCountryIdListAndCityIdListAsync(GetHotelsByCountryAndCityListRequest request)
        {
            List<int>? countryIds = request.countryIds;
            List<int>? cityIds = request.cityIds;

            List<HotelResponseModel>? result = null;
            try
            {
                //if (result != null && request.IncludeImages)
                if (request.IncludeImages)
                {

                    //var hotelIds1 = result.Select(r => r.hotel.Id).ToList();
                    //var hotelImages = _dbContext.HotelImage
                    //                .Where(hi => hotelIds1
                    //                .Contains(hi.HotelId))
                    //                .ToList();


                    //List<int> hotelsIds = result.Select(h => h.hotel.Id).Distinct().ToList();
                    //var images = await _dbContext.HotelImage
                    //                .Where(img => hotelsIds
                    //                .Contains(img.HotelId))
                    //                .ToListAsync();

                    result = await (from hotel in _dbContext.Hotel
                                    join ctry in _dbContext.Country
                                    on hotel.Country equals ctry.Id
                                    join cty in _dbContext.City
                                    on hotel.City equals cty.Id
                                    join hotelImages in _dbContext.HotelImage
                                    on hotel.Id equals hotelImages.HotelId into hotelImageGroup
                                    from hotelImages in hotelImageGroup.DefaultIfEmpty()
                                    where (countryIds == null || !countryIds.Any() || countryIds.Contains((int)hotel.Country))
                                          &&
                                          (cityIds == null || !cityIds.Any() || cityIds.Contains((int)hotel.City))
                                    select new HotelResponseModel()
                                    {
                                        hotel = hotel,
                                        hotelCityName = cty.Name,
                                        hotelCityNameEng = cty.NameEng,
                                        hotelCountryName = ctry.Name,
                                        hotelCountryNameEng = ctry.NameENG,
                                        hotelImages = _dbContext.HotelImage.Where(hi => hi.HotelId == hotel.Id).ToList()
                                        //hotelImages = hotelImageGroup.Where(hi => hi.HotelId == hotel.Id).ToList()
                                    }).ToListAsync();
                }
                else
                {
                    result = await (from hotel in _dbContext.Hotel
                                    join ctry in _dbContext.Country
                                    on hotel.Country equals ctry.Id
                                    join cty in _dbContext.City
                                    on hotel.City equals cty.Id
                                    where (countryIds == null || !countryIds.Any() || countryIds.Contains((int)hotel.Country))
                                          &&
                                          (cityIds == null || !cityIds.Any() || cityIds.Contains((int)hotel.City))
                                    select new HotelResponseModel()
                                    {
                                        hotel = hotel,
                                        hotelCityName = cty.Name,
                                        hotelCityNameEng = cty.NameEng,
                                        hotelCountryName = ctry.Name,
                                        hotelCountryNameEng = ctry.NameENG,
                                        hotelImages = null
                                    }).ToListAsync();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        public async Task<HotelResponseModel?> GetHotelByCodeAsync(string code)
        {
            HotelResponseModel? retValue = new();

            var hot = _dbContext.Hotel.FirstOrDefaultAsync(h => h.Code == code).Result;

            if (hot != null)
            {
                retValue.hotel = hot;

                var hotCountry = await _dbContext.Country.FirstOrDefaultAsync(c => c.Id == hot.Country);

                if (hotCountry != null)
                {
                    retValue.hotelCountryName = hotCountry.Name;
                    retValue.hotelCountryNameEng = hotCountry.NameENG;
                }

                var hotCity = await _dbContext.City.FirstOrDefaultAsync(c => c.Id == hot.City);

                if (hotCity != null)
                {
                    retValue.hotelCityName = hotCity.Name;
                    retValue.hotelCityNameEng = hotCity.NameEng;
                }

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

        public async Task<BookPayment> AddBookPaymentAsync(BookPayment payment)
        {
            await _dbContext.BookPayments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();

            return payment;
        }
    }


}
