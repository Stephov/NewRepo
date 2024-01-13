using Bogus;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Repositories.Concrete.Sansejour;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class FakeDataGenerationManager: IFakeDataGenerationManager
    {
        private Faker faker = new();
        private List<string> HotelCodes = new();
        private List<MaratukAdmin.Entities.Global.Country> Countries = new();
        //private readonly HotelImagesRepository _hotelImagesRepository;
        //private readonly CountryRepository _countryRepository;

        //public FakeDataGenerationManager(HotelImagesRepository hotelImagesRepository, CountryRepository countryRepository)
        //{
        //    _hotelImagesRepository= hotelImagesRepository;
        //    _countryRepository= countryRepository;
        //}
        public  List<RoomSearchResponse> GenerateFakeRooms(SearchFligtAndRoomRequest searchFligtAndRoomRequest, bool notRepeatableHotel)
        {
            var fakeRooms = new List<RoomSearchResponse>();
            string hotelCode = "";

            try
            {
                // Generate 10 fake records
                for (int i = 1; i <= 10; i++)
                {
                    //var fakeHotel = 
                    RoomSearchResponse fakeRoom = new()
                    {
                        SyncDate = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        //HotelCode = Faker.StringFaker.Numeric(3),
                        HotelCode = GenerateHotelCode(notRepeatableHotel),
                        HotelSeasonBegin = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        HotelSeasonEnd = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateTo,
                        RecID = faker.Random.String2(8, 8, "0123456789"),
                        CreateDate = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        ChangeDate = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        AccomodationPeriodBegin = searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        AccomodationPeriodEnd = searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        Room = faker.Random.String2(3, 3).ToUpper(),
                        RoomDesc = faker.Lorem.Word(),
                        RoomType = faker.Random.String2(3, 3).ToUpper(),
                        RoomTypeDesc = "STANDARD",
                        Board = "AI",
                        BoardDesc = "ALL INCLUSIVE",
                        RoomPax = searchFligtAndRoomRequest.RoomAdultCount + searchFligtAndRoomRequest.RoomChildCount,
                        RoomAdlPax = searchFligtAndRoomRequest.RoomAdultCount,
                        RoomChdPax = searchFligtAndRoomRequest.RoomChildCount,
                        AccmdMenTypeCode = faker.Random.String2(12, 12, "0123456789"),
                        AccmdMenTypeName = searchFligtAndRoomRequest.RoomAdultCount.ToString() + "Ad + " + searchFligtAndRoomRequest.RoomChildCount.ToString() + "Ch(Mek)(Erku)(Ereq)",
                        ReleaseDay = 0,
                        PriceType = "ROOM",
                        Price = faker.Random.Number(1, 5000),
                        WeekendPrice = null,
                        WeekendPercent = 0,
                        AccomLengthDay = "0-999",
                        Option = "Stay",
                        SpoNoApply = null,
                        SPOPrices = 2,
                        SPODefinit = "EEA// 04/08/2023 M",
                        NotCountExcludingAccomDate = "N",
                        HotelName = faker.Lorem.Word(),
                        HotelCategoryId = faker.Random.Number(1, 5),
                        HotelFileTypeId = faker.Random.Number(1, 2),
                        HotelFilePath = faker.Image.PlaceImgUrl(),
                        HotelAddress = faker.Address.ToString(),
                        HotelCheckIn = faker.Date.Recent(1),
                        HotelCheckOut = faker.Date.Recent(1),
                        HotelCityName = faker.Lorem.Word(),
                        HotelCityNameEng = faker.Lorem.Word(),
                        HotelCountryName = faker.Lorem.Word(),
                        HotelCountryNameEng = faker.Lorem.Word(),
                        HotelDescription = faker.Lorem.Word(),
                        HotelEmail = faker.Person.Email,
                        HotelFax = faker.Person.Phone
                        //HotelGpsLatitude =
                        //Address = faker.Address.FullAddress(),
                        //GpsLatitude = faker.Address.Latitude().ToString(),
                        //GpsLongitude = faker.Address.Longitude().ToString(),
                        //PhoneNumber = faker.Phone.PhoneNumber(),
                        //Fax = faker.Phone.PhoneNumber(),
                        //Email = faker.Internet.Email(),
                        //Site = "www." + faker.Lorem.Word() + ".com",
                        //Description = faker.Lorem.Sentence(10)


                    };

                    fakeRooms.Add(fakeRoom);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fakeRooms;
        }

        public string GenerateHotelCode(bool notRepeatableHotel)
        {
            //string hotelCode = Faker.StringFaker.Numeric(3);
            string hotelCode = faker.Random.String2(3, 3, "0123456789");

            if (notRepeatableHotel && HotelCodes.Contains(hotelCode))             // Hotel should NOT repeat
            {
                hotelCode = GenerateHotelCode(notRepeatableHotel);
            }

            if (!HotelCodes.Contains(hotelCode))
            {
                HotelCodes.Add(hotelCode);
            }

            return hotelCode;
        }

        public List<HotelImage> GenerateFakeHotelImages(int hotelId)
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

        public  HotelResponseModel GenerateFakeHotel(string code)
        {
            //HotelResponseModel fakeHotel;

            return new HotelResponseModel();
            //Hotels = GetAllHotelsAsync().Result;
            //Countries = _countryRepository.GetAllAsync().Result;
            //try
            //{
            //    int countryId;
            //    if (Countries == null)
            //    {
            //        countryId = faker.Random.Number(1, 100);
            //    }
            //    else
            //    {
            //        int countryIndex = faker.Random.Number(1, Countries.Count);
            //        countryId = Countries[countryIndex].Id;
            //    }

            //    // Generate fake info
            //    fakeHotel = new HotelResponseModel()
            //    {
            //        hotel = new Hotel()
            //        {
            //            Id = faker.Random.Number(1, 100),
            //            Code = code,
            //            Name = faker.Lorem.Word(),
            //            //Country = Faker.RandomNumber.Next(1, 5000),
            //            Country = countryId,
            //            City = faker.Random.Number(1, 100),
            //            HotelCategoryId = faker.Random.Number(1, 5),
            //            //IsCruise = faker.Random.Number(0, 1),
            //            IsCruise = faker.Random.Byte(0, 1),

            //            Address = faker.Address.FullAddress(),
            //            GpsLatitude = faker.Address.Latitude().ToString(),
            //            GpsLongitude = faker.Address.Longitude().ToString(),
            //            PhoneNumber = faker.Phone.PhoneNumber(),
            //            Fax = faker.Phone.PhoneNumber(),
            //            Email = faker.Internet.Email(),
            //            Site = "www." + faker.Lorem.Word() + ".com",
            //            Description = faker.Lorem.Sentence(10)
            //        }
            //    };

            //    // Generate fake hotel images
            //    var hotImages = _hotelImagesRepository.GetHotelImagesByHotelIdMockAsync(fakeHotel.hotel.Id).Result;

            //    fakeHotel.hotelImages = hotImages;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //return fakeHotel;
        }

    }
}
