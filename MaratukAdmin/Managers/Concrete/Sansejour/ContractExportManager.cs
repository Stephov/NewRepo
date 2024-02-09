using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using MaratukAdmin.Repositories.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System;
using System.Xml.Serialization;
using MaratukAdmin.Models.Requests;
using System.Globalization;
using System.Collections.Generic;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Entities;
using Org.BouncyCastle.Utilities;
using System.Collections;
//using System.Transactions;
//using MaratukAdmin.Infrastructure;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class ContractExportManager : IContractExportManager
    {
        private readonly IMainRepository<SyncSejourContractExportView> _mainRepository;
        //private readonly IMapper _mapper;
        private readonly IContractExportRepository _contractExportRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IHttpRequestManager _httpRequestManager;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDistributedCache _cache;
        private readonly IPriceBlockManager _priceBlockManager;
        //protected readonly MaratukDbContext _dbContext;


        public ContractExportManager(IMainRepository<SyncSejourContractExportView> mainRepository,
                            //IMapper mapper,
                            IContractExportRepository contractExportRepository,
                            IHotelRepository hotelRepository,
                            IHttpRequestManager httpRequestManager,
                            ITransactionRepository transactionRepository,
                            IDistributedCache cache,
                            IPriceBlockManager priceBlockManager
            //MaratukDbContext dbContext

            )
        {
            _mainRepository = mainRepository;
            //_mapper = mapper;
            _contractExportRepository = contractExportRepository;
            _hotelRepository = hotelRepository;
            _httpRequestManager = httpRequestManager;
            _transactionRepository = transactionRepository;
            _cache = cache;
            _priceBlockManager = priceBlockManager;
            //_dbContext = dbContext;
        }


        public async Task<DateTime?> GetMaxSyncDateAsyncNew(DbContext dbContext)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var maxSyncDate = await _contractExportRepository.GetMaxSyncDateAsync();
                    await transaction.CommitAsync();
                    return maxSyncDate;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> GetSejourContractExportViewWorker(int? syncByChangedHotels, string? hotelCode, int currentTry = 0)
        {
            int retryCount = 5;
            bool retValue = false;
            try
            {
                retValue = await GetSejourContractExportView(syncByChangedHotels, hotelCode);
                if (retValue == false)
                {
                    currentTry++;
                    if (currentTry == retryCount)
                    { retValue = false; }
                    else
                    { await GetSejourContractExportViewWorker(syncByChangedHotels, hotelCode, currentTry); }
                }
            }
            catch (Exception)
            {
                currentTry++;
                if (currentTry == retryCount)
                { retValue = false; }
                else
                { await GetSejourContractExportViewWorker(syncByChangedHotels, hotelCode, currentTry); }
            }
            return retValue;
        }

        //public async Task<bool> GetSejourContractExportView(List<HotelSansejourResponse>? hotelsList = null)
        public async Task<bool> GetSejourContractExportView(int? syncByChangedHotels, string? hotelCode)
        {
            bool retValue;
            bool contractExportViewRecorded = false;
            //bool previousDataDeleted = false;
            //bool syncByChangedHotels = false;
            int skippedHotels = 0;
            int processedHotels = 0;
            int existedHotels = 0;
            string dateString = "";
            string syncDateFormat = "";
            string processingHotelCode = "";
            DateTime syncDate = DateTime.MinValue;
            DateTime dt;
            bool syncDateIsKnown = false;
            DateTime oldSyncDate = DateTime.MinValue;

            DateTime dateStartSession = DateTime.Now;
            DateTime dateStartLoop = DateTime.Now;

            DateTime dateStartHtmlQuery = DateTime.Now;
            TimeSpan elapsedHtmlQuery;
            DateTime dateStartOtherOperations = DateTime.Now;
            TimeSpan elapsedOtherOperations;

            TimeSpan elapsedHtmlQueryTotal = new TimeSpan();
            TimeSpan elapsedOtherOperationsTotal = new TimeSpan();

            TimeSpan elapsed;
            List<string> hotelsList = new();
            bool dataWasArchived = false;
            List<int> neededCountryCodes = new List<int> { 6226 };
            List<int> neededCityCodes = new List<int> { 26, 85 };


            try
            {
                await _httpRequestManager.LoginAsync();
                string token = await _cache.GetStringAsync("token");

                //throw new Exception("test exception");

                if (hotelCode == null)
                {
                    string beginDate = DateTime.Now.ToString("yyyy-MM-dd");

                    if (syncByChangedHotels == 1)
                    {                                                                                           // *** Sync data only by Changed Hotels list
                        hotelsList = await _httpRequestManager.GetChangedHotelListSansejourAsync(beginDate);

                        while (hotelsList.Count == 0)
                        {
                            if (DateTime.TryParseExact(beginDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                dt = dt.AddDays(-1);
                                beginDate = dt.ToString("yyyy-MM-dd");

                                hotelsList = await _httpRequestManager.GetChangedHotelListSansejourAsync(beginDate);
                            }
                        }
                    }
                    else
                    {                                                                                           // *** Sync data by Full list of Hotels
                        var hotels = await _httpRequestManager.GetAllHotelsSansejourAsync();
                        hotelsList = hotels.Select(h => h.Code).ToList();

                        while (hotelsList.Count == 0)
                        {
                            if (DateTime.TryParseExact(beginDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                dt.AddDays(-1);
                                beginDate = dt.ToString("yyyy-MM-dd");

                                hotels = await _httpRequestManager.GetAllHotelsSansejourAsync();
                                hotelsList = hotels.Select(h => h.Code).ToList();
                            }
                        }
                    }
                }
                else
                { hotelsList.Add(hotelCode); }

                hotelsList = hotelsList.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x).ToList();

                if (hotelsList == null || hotelsList.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Hotels list is EMPTY");
                    return false;
                }

                // *** Get only necessary Hotels
                var neededHotelsList = await _hotelRepository.GetHotelsByCountryIdAndCityIdAsync(neededCountryCodes, neededCityCodes);

                if (neededHotelsList != null)
                { hotelsList = hotelsList.Where(h => neededHotelsList.Any(hotel => hotel.Code == h)).ToList(); }

                dateStartLoop = DateTime.Now;

                //var strategy = _transactionRepository.CreateExecutionStrategy();
                //await strategy.ExecuteAsync(async () =>
                //{
                //    await _transactionRepository.BeginTransAsync();                                             // Begin transaction
                //    {

                //var result = await _contractExportRepository.GetMaxSyncDateFromSejourRateAsync();
                //if (result != null)
                //{ oldSyncDate = result.Value; }

                // *** ARCHIVE SyncSejourRate table old data ***
                if (string.IsNullOrWhiteSpace(hotelCode))
                {
                    // todo NAEL
                    //var archiveResult = await _contractExportRepository.ArchiveSyncSejourRateData(oldSyncDate);
                    //if (!archiveResult)
                    //{ throw new Exception("Error archiving SyncSejourRate data"); }
                    dataWasArchived = true;
                }

                // *** Loop for HOTELS ***
                foreach (var hotel in hotelsList)
                {
                    dateStartLoop = DateTime.Now;
                    //processingHotelCode = hotel.Code;
                    processingHotelCode = hotel;
                    System.Diagnostics.Debug.WriteLine($"--- PROCESSING --- Hotel Code: {processingHotelCode}");

                    GetSejourContractExportViewRequestModel reqModel = new()
                    {
                        Token = token,
                        //Season = "W23",
                        //HotelCode = hotel.Code
                        HotelCode = hotel
                    };

                    if (!syncDateIsKnown)
                    {
                        // Firts Hotel always fills because of not knowing sync date
                    }
                    if (syncDateIsKnown)
                    {
                        // *** Check Rates existence for this Hotel and SyncDate
                        bool isHotelExists = await _contractExportRepository.GetSyncSejourRateExistenceByDateAndHotelAsync(hotel, syncDate);

                        if (isHotelExists)
                        {
                            //previousDataDeleted = true;
                            existedHotels++;
                            continue;
                        }
                    }

                    dateStartHtmlQuery = DateTime.Now;
                    SyncSejourContractExportViewResponse sejourContracts = await _httpRequestManager.GetSejourContractExportViewAsync(reqModel);
                    elapsedHtmlQuery = DateTime.Now - dateStartHtmlQuery;
                    elapsedHtmlQueryTotal += elapsedHtmlQuery;

                    if (sejourContracts == null || sejourContracts.Body == null || sejourContracts.Body.GetSejourContractExportViewResponse == null
                        || sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult == null
                        || sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data == null
                        || sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export == null
                        || sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel == null
                        || sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.SpecialOffers == null
                        || sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.SpecialOffers.SpecialOffers == null
                        || sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.SpecialOffers.SpecialOffers == null
                        )
                    {
                        //throw new Exception("Could not get contracts");
                        skippedHotels++;
                        //System.Diagnostics.Debug.WriteLine($"--- SKIP --- Hotel Code: {hotel.Code}");
                        System.Diagnostics.Debug.WriteLine($"--- SKIP --- Hotel Code: {hotel}");
                        continue;
                    }

                    //DateTime? periodBegin = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.SpecialOffers.SpecialOffers[0].Rates[0].AccomodationPeriodBegin;
                    //DateTime? periodEnd = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.SpecialOffers.SpecialOffers[0].Rates[0].AccomodationPeriodEnd;

                    // *** Check for correct AccomodationPeriodBegin and AccomodationPeriodEnd values
                    bool isError = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.SpecialOffers.SpecialOffers
                                    .Any(offer => offer.Rates.Any(obj => (DateTime?)obj.AccomodationPeriodBegin > (DateTime?)obj.AccomodationPeriodEnd));
                    if (isError)
                    { throw new Exception("-- *** Wrong Date deserialization!!! *** --"); }

                    dateString = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date;
                    syncDateFormat = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.DateFormat
                                    .Replace("mm", "MM");

                    //dateString = "28/09/2023";
                    syncDateFormat = "dd/MM/yyyy";
                    syncDate = DateTime.ParseExact(dateString, syncDateFormat, CultureInfo.InvariantCulture);
                    syncDateIsKnown = true;

                    dateStartOtherOperations = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine($"--- DELETING PREVIOUS DATA ---");

                    // Delete previous data by HotelCode (if we reach this line that means data was found for this Hotel)
                    //if (syncByChangedHotels == 1 || !string.IsNullOrWhiteSpace(hotelCode))
                    //{
                    // todo avtomat zapusk anel es funkcian
                    // todo namaki texty dzel
                    // CountryId ev CityId avelacnel LowesPrices searchi mej

                    // todo + ChangedHotelList cucakic vercnel miayn 6226 Country ev Sharm, Hurgada City-ner
                    // todo + ChangedHotelList - ic price tarmacnelu jamanak ete tvyal chi gtnum, apa hin toghery petk chi heracnel. Heracvum en miayn en depqum, erb nor togher gtel enq.
                    // todo + Price tarmacnogh funkcian kanchel ayl funkciayic, vortex stugel sxali haytnvely u 5 angam porcel sharunakel
                    // todo + Price tarmacnelu yntacqum transaction petk el chi. Inchqany hascrec togh gri. Hetaga zapuskneri jamanak kareliya stugel, ete trvac SyncDate ev HotelCode-ov togher kan, apa dranq toghnel tenc ev ancnel myus Hotelin.
                    // todo + Mek Hoteli tvyal kam ChangedHotelList-ov tarmacman ardyunqum petk chi SyncDate update anel, togh linen tarber.SyncSejourRate tablican khamarvi yntacik, ev poiski jamanak SyncDate kareli e hashvi charnel.
                    // todo + Searchi jamanak SyncDate-in chnael, qani vor SyncSejourRate tablicayum arden karox en linel tarber amsatverov togher
                    // todo - Schedule(worker) vor tvyalnery tarmacni.Sxali depqum eli porci, Log gri inch-vor tablicayi mej, u noric porci kardal, asenq 5 angam
                    // todo - Amen or veronshyal Workeri ashxatanqi jamanak naev stugel AccomodationPeriodEnd-y ete ancel e, apa jnjel et Hoteli tvyalnery

                    //var deleteResult = await _contractExportRepository.DeleteSyncedDataByDateAsync(syncDate);
                    //var deleteResult = await _contractExportRepository.DeleteSyncedDataByHotelCodeAsync(syncDate, hotel);
                    //var deleteResult = await _contractExportRepository.DeleteSyncedDataBySyncDateAndHotelCodeAsync(oldSyncDate, hotel);

                    // Delete all data for this Hotel
                    //var deleteResult = await _contractExportRepository.DeleteSyncedDataBySyncDateAndHotelCodeAsync(default, hotel);

                    //if (syncDateIsKnown)
                    //{
                    //    // *** Check Rates existence for this Hotel and SyncDate
                    //    bool isHotelExists = await _contractExportRepository.GetSyncSejourRateExistenceByDateAndHotelAsync(hotel, syncDate);

                    //    if (isHotelExists)
                    //    {
                    //        //previousDataDeleted = true;
                    //        existedHotels++;
                    //        continue;
                    //    }
                    //}

                    // Delete all data for this Hotel, EXCEPT EXISTING
                    var deleteResult = await _contractExportRepository.DeleteSyncedDataByHotelCodeExceptSyncDateAsync(syncDate, hotel);

                    if (!deleteResult)
                    { throw new Exception("Error deleting previous data"); }

                    //}
                    // Delete previous data by Sync Date
                    //else if ((syncByChangedHotels == null || syncByChangedHotels == 0) && !previousDataDeleted)
                    //{
                    //    //var deleteResult = await _contractExportRepository.DeleteSyncedDataByDateAsync((DateTime)sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date);
                    //    var deleteResult = await _contractExportRepository.DeleteSyncedDataByDateAsync(syncDate);
                    //    if (!deleteResult)
                    //    { throw new Exception("Error deleting previous data"); }
                    //    previousDataDeleted = true;
                    //}

                    string jsonSejourContracts = JsonConvert.SerializeObject(sejourContracts);

                    SyncSejourContractExportView contract = new()
                    {
                        Version = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Version,
                        //ExportDate = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date,
                        //ExportDate = (string.IsNullOrWhiteSpace(hotelCode)) ? syncDate : oldSyncDate,
                        ExportDate = syncDate,
                        //DateFormat = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.DateFormat,
                        DateFormat = syncDateFormat,
                        SanBeginDate = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.SanBeginDate,
                        SanEndDate = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.SanEndDate,
                    };

                    SyncSejourHotel syncSejourHotel = new()
                    {
                        //SyncDate = (DateTime)sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date,
                        //SyncDate = (string.IsNullOrWhiteSpace(hotelCode)) ? syncDate : oldSyncDate,
                        SyncDate = syncDate,
                        HotelCode = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelCode,
                        HotelName = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelName,
                        HotelCategory = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelCategory,
                        //HotelAddress = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelAddress,
                        HotelRegionCode = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelRegionCode,
                        HotelRegion = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelRegion,
                        HotelTrfRegionCode = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelTrfRegionCode,
                        HotelTrfRegion = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelTrfRegion,
                        AllotmentType = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.AllotmentType,
                        Currency = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.Currency,
                        HotelSeason = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelSeason,
                        HotelSeasonBegin = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelSeasonBegin,
                        HotelSeasonEnd = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelSeasonEnd,
                        HotelType = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.HotelType,
                        ChildAgeCalculationOrder = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.General.ChildAgeCalculationOrder
                    };

                    List<SyncSejourSpoAppOrder> sejourSpoAppOrders = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult
                                                .Data.Export.Hotel.SpecialOffers.SpoAppOrders.SpoAppOrder.Select(appOrder => new SyncSejourSpoAppOrder
                                                {
                                                    //SyncDate = (string.IsNullOrWhiteSpace(hotelCode)) ? syncSejourHotel.SyncDate : oldSyncDate,
                                                    SyncDate = syncSejourHotel.SyncDate,
                                                    HotelCode = syncSejourHotel.HotelCode,
                                                    HotelSeasonBegin = syncSejourHotel.HotelSeasonBegin,
                                                    HotelSeasonEnd = syncSejourHotel.HotelSeasonEnd,
                                                    SpoCode = appOrder.SpoCode,
                                                    SpoOrder = appOrder.SpoOrder
                                                }).ToList();

                    List<SyncSejourSpecialOffer> syncSejourSpecialOffers = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult
                                                .Data.Export.Hotel.SpecialOffers.SpecialOffers.Select(specOffer => new SyncSejourSpecialOffer
                                                {
                                                    //SyncDate = (string.IsNullOrWhiteSpace(hotelCode)) ? syncSejourHotel.SyncDate : oldSyncDate,
                                                    SyncDate = syncSejourHotel.SyncDate,
                                                    HotelCode = syncSejourHotel.HotelCode,
                                                    HotelSeasonBegin = syncSejourHotel.HotelSeasonBegin,
                                                    HotelSeasonEnd = syncSejourHotel.HotelSeasonEnd,
                                                    SpoCode = specOffer.SpoCode,
                                                    SpoNo = specOffer.SpoNo,
                                                    SalePeriodBegin = specOffer.SalePeriodBegin,
                                                    SalePeriodEnd = specOffer.SalePeriodEnd,
                                                    //SpecialCode = specOffer.SpecialCode
                                                }).ToList();

                    List<SyncSejourRate> syncSejourRates = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult
                                                .Data.Export.Hotel.SpecialOffers.SpecialOffers.SelectMany(offer => offer.Rates)
                                                .Select(sejRate => new SyncSejourRate
                                                {
                                                    //SyncDate = (string.IsNullOrWhiteSpace(hotelCode)) ? syncSejourHotel.SyncDate : oldSyncDate,
                                                    SyncDate = syncSejourHotel.SyncDate,
                                                    HotelCode = syncSejourHotel.HotelCode,
                                                    HotelSeasonBegin = syncSejourHotel.HotelSeasonBegin,
                                                    HotelSeasonEnd = syncSejourHotel.HotelSeasonEnd,
                                                    RecID = sejRate.RecID,
                                                    CreateDate = sejRate.CreateDate,
                                                    ChangeDate = sejRate.ChangeDate,
                                                    AccomodationPeriodBegin = sejRate.AccomodationPeriodBegin,
                                                    AccomodationPeriodEnd = sejRate.AccomodationPeriodEnd,
                                                    Room = sejRate.Room,
                                                    RoomDesc = sejRate.RoomDesc,
                                                    RoomType = sejRate.RoomType,
                                                    RoomTypeDesc = sejRate.RoomTypeDesc,
                                                    Board = sejRate.Board,
                                                    BoardDesc = sejRate.BoardDesc,
                                                    RoomPax = sejRate.RoomPax,
                                                    RoomAdlPax = sejRate.RoomAdlPax,
                                                    RoomChdPax = sejRate.RoomChdPax,
                                                    // Adults and Child count & Ages
                                                    AccmdMenTypeCode = FormatAccmdMenTypeCode(sejRate.RoomAdlPax, sejRate.RoomChdPax, sejRate.ChildAges),
                                                    AccmdMenTypeName = FormatAccmdMenTypeName(sejRate.RoomAdlPax, sejRate.RoomChdPax, sejRate.ChildAges),
                                                    ReleaseDay = sejRate.ReleaseDay,
                                                    PriceType = sejRate.PriceType,
                                                    Price = Math.Ceiling((double)sejRate.Price),
                                                    ////Percent = sejRate.Percent,
                                                    ////WeekendPrice = sejRate.WeekendPrice,
                                                    WeekendPercent = (double)sejRate.WeekendPercent,
                                                    AccomLengthDay = sejRate.AccomLengthDay,
                                                    Option = sejRate.Option,
                                                    ////SpoNoApply = sejRate.SpoNoApply,
                                                    SPOPrices = (double)sejRate.SPOPrices,
                                                    SPODefinit = sejRate.SPODefinit,
                                                    NotCountExcludingAccomDate = sejRate.NotCountExcludingAccomDate
                                                }).ToList();

                    //List<SyncSejourChildAges> syncRateChildAges = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult
                    //                            .Data.Export.Hotel.SpecialOffers.SpecialOffers
                    //                                .SelectMany(offer => offer.Rates)
                    //                                //.Select(rates => new SyncSejourChildAges()
                    //                                .SelectMany(rates => rates.ChildAges, (rates, childAges) => new SyncSejourChildAges
                    //                                //.SelectMany(rates => new SyncSejourChildAges()
                    //                                //.SelectMany(offer => offer.Rates)
                    //                                //.SelectMany(rate => rate.subRates, (rate, subRate) => new SubRateModel
                    //                                {
                    //                                    SyncDate = syncSejourHotel.SyncDate,
                    //                                    HotelCode = syncSejourHotel.HotelCode,
                    //                                    HotelSeasonBegin = syncSejourHotel.HotelSeasonBegin,
                    //                                    HotelSeasonEnd = syncSejourHotel.HotelSeasonEnd,
                    //                                    RateRecID = rates.RecID,
                    //                                    C1Age1 = (double)childAges.C1Age1,
                    //                                    C1Age2 = (double)childAges.C1Age2,
                    //                                    C2Age1 = (double)childAges.C2Age1,
                    //                                    C2Age2 = (double)childAges.C2Age2,
                    //                                    C3Age1 = (double)childAges.C3Age1,
                    //                                    C3Age2 = (double)childAges.C3Age2,
                    //                                    C4Age1 = (double)childAges.C4Age1,
                    //                                    C4Age2 = (double)childAges.C4Age2 
                    //                                }).ToList();


                    if (!contractExportViewRecorded)
                    {
                        await _contractExportRepository.DeleteSyncSejourContractsByDateAsync(syncSejourHotel.SyncDate);
                        await _contractExportRepository.AddlNewSejourContractsAsync(contract);
                        contractExportViewRecorded = true;
                    }

                    await _contractExportRepository.AddNewSejourHotelsAsync(syncSejourHotel);

                    sejourSpoAppOrders = sejourSpoAppOrders.Where(item => item != null && item.SyncDate == syncDate).ToList();
                    await _contractExportRepository.AddNewSejourSpoAppOrdersAsync(sejourSpoAppOrders);

                    syncSejourSpecialOffers = syncSejourSpecialOffers.Where(item => item != null && item.SyncDate == syncDate).ToList();
                    await _contractExportRepository.AddNewSejourSpecialOffersAsync(syncSejourSpecialOffers);

                    syncSejourRates = syncSejourRates.Where(item => item != null && item.SyncDate == syncDate).ToList();
                    await _contractExportRepository.AddNewSejourRatesAsync(syncSejourRates);

                    elapsedOtherOperations = DateTime.Now - dateStartOtherOperations;
                    elapsedOtherOperationsTotal += elapsedOtherOperations;

                    processedHotels++;
                    elapsed = DateTime.Now - dateStartLoop;
                    System.Diagnostics.Debug.WriteLine($"--- DONE --- Hotel Code: {hotel}, elapsed: {elapsed.Seconds} seconds");
                    System.Diagnostics.Debug.WriteLine($"--- Elapsed -- Html query: {elapsedHtmlQuery.Seconds} seconds, Other operations: {elapsedOtherOperations.Seconds} seconds");
                    elapsed = DateTime.Now - dateStartSession;
                    System.Diagnostics.Debug.WriteLine($"--- TOTAL --- SKIPPED: {skippedHotels}, PROCESSED: {processedHotels}, Elapsed: {elapsed.Minutes} minutes, {elapsed.Seconds} seconds");
                }

                #region AccomodationTypes
                // *** GET possible AccomodationTypes from Rates 
                List<SyncSejourAccomodationType> accmdTypes = await _contractExportRepository.GetSyncSejourAccomodationTypesFromRatesBySyncDateAsync(syncDate);

                // ** Add gathered AccomodationTypes to DB
                await _contractExportRepository.AddNewSyncSejourAccomodationTypesAsync(accmdTypes);

                //(int, int) counts = await _contractExportRepository.DescribeListOfAccomodationTypesAsync(accmdTypes);
                (int, int) counts = await DescribeAccomodationTypes(accmdTypes);
                #endregion

                #region HotelBoards
                // *** GET possible HotelBoards from Rates
                List<HotelBoard> hotelBoards = await _contractExportRepository.GetHotelBoardsFromRatesBySyncDateAsync(syncDate);

                // ** Add gathered HotelBoards to DB
                await _contractExportRepository.AddNewHotelBoardsAsync(hotelBoards);
                #endregion

                //if (string.IsNullOrWhiteSpace(hotelCode))
                //{
                //    // *** Update SyncSejour SyncDate
                //    //var updateResult = await _contractExportRepository.UpdateSyncSejourRateSyncDateAsync(syncDate);
                //    var updateResult = await _contractExportRepository.UpdateSyncSejourRateSyncDateRAWAsync(syncDate);
                //    if (!updateResult)
                //    { throw new Exception("Error updating SyncSejourRate's SyncDate"); }
                //}

                elapsed = DateTime.Now - dateStartSession;
                System.Diagnostics.Debug.WriteLine($"--- FINISHED --- SKIPPED: {skippedHotels}, PROCESSED: {processedHotels}, EXISTED: {existedHotels}");
                System.Diagnostics.Debug.WriteLine($"--- ELAPSED on Html queries: {elapsedHtmlQueryTotal.Minutes} minutes, {elapsedHtmlQueryTotal.Seconds} seconds");
                System.Diagnostics.Debug.WriteLine($"--- ELAPSED on Other operations: {elapsedOtherOperationsTotal.Minutes} minutes, {elapsedOtherOperationsTotal.Seconds} seconds");
                System.Diagnostics.Debug.WriteLine($"--- ELAPSED TOTAL: {elapsed.Hours} hour(s), {elapsed.Minutes} minutes, {elapsed.Seconds} seconds");
                System.Diagnostics.Debug.WriteLine($"--- AVERAGE TOTAL: {Math.Round((elapsed.TotalSeconds / (processedHotels + skippedHotels)), 2)} seconds");
                System.Diagnostics.Debug.WriteLine($"--- Total Accomodations: {counts.Item1}, Described: {counts.Item2}");

                //await _transactionRepository.CommitTransAsync();                                            // Commit transaction
                //    }
                //});


                retValue = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message} Hotel Code: {processingHotelCode}");
                retValue = false;

            }
            return retValue;
        }

        public async Task<(int, int)> DescribeAccomodationTypes(List<SyncSejourAccomodationType> accmdTypeList)
        {
            try
            {
                return await _contractExportRepository.DescribeListOfAccomodationTypesAsync(accmdTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string FormatAccmdMenTypeCode(int adultCount, int childCount, ChildAges chAges)
        //public string FormatAccmdMenTypeCode(int adultCount, int childCount, List<ChildAges> chAges)
        {
            string retValue = "";
            try
            {
                retValue = (adultCount > 0) ? (adultCount.ToString("00")) : "";
                retValue += (childCount > 0) ? childCount.ToString("00") : "";

                if (chAges != null) // && chAges.Count > 0)
                {
                    //foreach (var child in chAges)
                    //{
                    //    retValue += Math.Round((decimal)child.C1Age1).ToString("00") + Math.Round((decimal)child.C1Age2).ToString("00");
                    //}
                    if (chAges.C1Age1 != null && chAges.C1Age2 != null)
                    { retValue += Math.Round((decimal)chAges.C1Age1).ToString("00") + Math.Round((decimal)chAges.C1Age2).ToString("00"); }

                    if (chAges.C2Age1 != null && chAges.C2Age2 != null)
                    { retValue += Math.Round((decimal)chAges.C2Age1).ToString("00") + Math.Round((decimal)chAges.C2Age2).ToString("00"); }

                    if (chAges.C3Age1 != null && chAges.C3Age2 != null)
                    { retValue += Math.Round((decimal)chAges.C3Age1).ToString("00") + Math.Round((decimal)chAges.C3Age2).ToString("00"); }

                    if (chAges.C4Age1 != null && chAges.C4Age2 != null)
                    { retValue += Math.Round((decimal)chAges.C4Age1).ToString("00") + Math.Round((decimal)chAges.C4Age2).ToString("00"); }

                    if (chAges.C5Age1 != null && chAges.C5Age2 != null)
                    { retValue += Math.Round((decimal)chAges.C5Age1).ToString("00") + Math.Round((decimal)chAges.C5Age2).ToString("00"); }
                }
            }
            catch (Exception)
            {
                retValue = "";
            }
            return retValue;
        }

        public string FormatAccmdMenTypeName(int adultCount, int childCount, ChildAges chAges)
        //public string FormatAccmdMenTypeName(int adultCount, int childCount, List<ChildAges> chAges)
        {
            string retValue = "";
            try
            {
                retValue = (adultCount > 0) ? (adultCount.ToString() + "Ad") : "";

                if (chAges != null) // && chAges.Count > 0)
                {
                    retValue += (childCount > 0) ? (" + " + childCount.ToString() + "Ch") : "";

                    //foreach (var child in chAges)
                    //{
                    //    retValue += "(" + Math.Round((decimal)child.C1Age1).ToString() + "-" + Math.Round((decimal)child.C1Age2).ToString() + ")";
                    //}

                    if (chAges.C1Age1 != null && chAges.C1Age2 != null)
                    { retValue += "(" + Math.Round((decimal)chAges.C1Age1).ToString() + "-" + Math.Round((decimal)chAges.C1Age2).ToString() + ")"; }

                    if (chAges.C2Age1 != null && chAges.C2Age2 != null)
                    { retValue += (string.IsNullOrWhiteSpace(retValue)) ? "" : "(" + Math.Round((decimal)chAges.C2Age1).ToString() + "-" + Math.Round((decimal)chAges.C2Age2).ToString() + ")"; }

                    if (chAges.C3Age1 != null && chAges.C3Age2 != null)
                    { retValue += (string.IsNullOrWhiteSpace(retValue)) ? "" : "(" + Math.Round((decimal)chAges.C3Age1).ToString() + "-" + Math.Round((decimal)chAges.C3Age2).ToString() + ")"; }

                    if (chAges.C4Age1 != null && chAges.C4Age2 != null)
                    { retValue += (string.IsNullOrWhiteSpace(retValue)) ? "" : "(" + Math.Round((decimal)chAges.C4Age1).ToString() + "-" + Math.Round((decimal)chAges.C4Age2).ToString() + ")"; }

                    if (chAges.C5Age1 != null && chAges.C5Age2 != null)
                    { retValue += (string.IsNullOrWhiteSpace(retValue)) ? "" : "(" + Math.Round((decimal)chAges.C5Age1).ToString() + "-" + Math.Round((decimal)chAges.C5Age2).ToString() + ")"; }
                }
            }
            catch (Exception)
            {
                retValue = "";
            }
            return retValue;
        }
        public static T DeserializeXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public async Task<List<SyncSejourRate>> SearchRoomOldAsync(SearchRoomRequest searchRequest)
        {
            try
            {
                return await _contractExportRepository.SearchRoomOldAsync(searchRequest);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                throw;
            }
        }

        //public async Task<List<SyncSejourRate>> SearchRoomAsync(SearchRoomRequest searchRequest)
        public async Task<List<RoomSearchResponse>> SearchRoomAsync(SearchRoomRequest searchRequest)
        {
            try
            {
                return await _contractExportRepository.SearchRoomAsync(searchRequest);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                throw;
            }
        }

        //public async Task<List<SyncSejourRate>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest)
        public async Task<List<RoomSearchResponseLowestPrices>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest)
        {
            try
            {
                return await _contractExportRepository.SearchRoomLowestPricesAsync(searchRequest);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                throw;
            }
        }

        public async Task<List<SearchFligtAndRoomResponse>> SearchFlightAndRoomAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        {
            int flightAdultCount = searchFlightAndRoomRequest.FlightAdult;
            int flightChildCount = 0;
            int flightInfantCount = 0;
            List<SearchFligtAndRoomResponse> retValue = new();

            // Define child counts and ages for Flight seach
            if (searchFlightAndRoomRequest.RoomChildAges != null)
            {
                foreach (var man in searchFlightAndRoomRequest.RoomChildAges)
                {
                    if (man <= 2)
                    { flightInfantCount++; }
                    else if (man > 2 && man <= 12)
                    { flightChildCount++; }
                    else if (man > 12)
                    { flightAdultCount++; }
                }
            }

            SearchFlightResult searchFlightRequest = new()
            {
                FlightOneId = searchFlightAndRoomRequest.FlightOneId,
                FlightTwoId = searchFlightAndRoomRequest.FlightTwoId,
                StartDate = searchFlightAndRoomRequest.FlightStartDate,
                ReturnedDate = searchFlightAndRoomRequest.FlightReturnedDate,
                Adult = flightAdultCount,   // searchFlightAndRoomRequest.FlightAdult,
                Child = flightChildCount,   // searchFlightAndRoomRequest.FlightChild,
                Infant = flightInfantCount  // searchFlightAndRoomRequest.FlightInfant
            };

            // Get FLIGHTS
            List<FinalFlightSearchResponse> resultFlightSearch = await _priceBlockManager.GetFligthSearchResultAsync(searchFlightRequest);
            //List<FinalFlightSearchResponse> resultFlightSearch = await _priceBlockManager.GetFligthSearchResultMockAsync(searchFlightRequest);          // MOCK

            SearchRoomRequest searchRoomRequest = new()
            {
                AccomodationDateFrom = searchFlightAndRoomRequest.RoomAccomodationDateFrom,
                AccomodationDateTo = searchFlightAndRoomRequest.RoomAccomodationDateTo,
                //Board = searchFlightAndRoomRequest.Board,
                AdultCount = searchFlightAndRoomRequest.RoomAdultCount,
                ChildCount = searchFlightAndRoomRequest.RoomChildCount,
                ChildAges = searchFlightAndRoomRequest.RoomChildAges,
                PageNumber = searchFlightAndRoomRequest.PageNumber,
                PageSize = searchFlightAndRoomRequest.PageSize,
                LateCheckout = searchFlightAndRoomRequest.LateCheckout,
                HotelCodes = searchFlightAndRoomRequest.HotelCodes,
                //HotelCategoryIds = searchFlightAndRoomRequest.HotelCategoryIds,
                //HotelCountryIds = searchFlightAndRoomRequest.HotelCountryIds,
                //HotelCityIds = searchFlightAndRoomRequest.HotelCityIds,
                //TotalPriceMin = searchFlightAndRoomRequest.TotalPriceMin,
                //TotalPriceMax = searchFlightAndRoomRequest.TotalPriceMax
            };

            // Get ROOMS
            var resultRoomSearch = await _contractExportRepository.SearchRoomAsync(searchRoomRequest);

            resultFlightSearch.ForEach(flight => flight.TotalPrice = Math.Ceiling((double)flight.TotalPrice));
            resultRoomSearch.ForEach(room => room.Price = Math.Ceiling((double)room.Price));

            // Combine results
            foreach (var flight in resultFlightSearch)
            {
                foreach (var room in resultRoomSearch)
                {
                    retValue.Add(new SearchFligtAndRoomResponse()
                    {
                        flightSearchResponse = flight,
                        roomSearchResponse = room,
                        flightAndRoomTotalPrice = Math.Ceiling((double)flight.TotalPrice + (double)room.Price)

                        //// Flight part
                        //Airline = flight.Airline,
                        //ArrivalTime = flight.ArrivalTime,
                        //CostPerTickets = flight.CostPerTickets,
                        //DepartureAirportCode = flight.DepartureAirportCode,
                        //DepartureTime = flight.DepartureTime,
                        //DestinationAirportCode = flight.DestinationAirportCode,
                        //DurationHours = flight.DurationHours,
                        //DurationMinutes = flight.DurationMinutes,
                        //FlightId = flight.FlightId,
                        //FlightNumber = flight.FlightNumber,
                        //NumberOfTravelers = flight.NumberOfTravelers,
                        //ReturnedFlight = flight.ReturnedFlight,
                        //AdultPrice = flight.AdultPrice,
                        //ChildPrice = flight.ChildPrice,
                        //InfantPrice = flight.InfantPrice,
                        //TotalPrice = flight.TotalPrice,

                        //// Room part
                        //Room = room.Room,
                        //RoomAccmdMenTypeCode = room.AccmdMenTypeCode,
                        //RoomAccmdMenTypeName = room.AccmdMenTypeName,
                        //RoomAccomLengthDay = room.AccomLengthDay,
                        //RoomAccomodationPeriodBegin = room.AccomodationPeriodBegin,
                        //RoomAccomodationPeriodEnd = room.AccomodationPeriodEnd,
                        //RoomAdlPax = room.RoomAdlPax,
                        //RoomBoard = room.Board,
                        //RoomBoardDesc = room.BoardDesc,
                        //RoomChangeDate = room.ChangeDate,
                        //RoomChdPax = room.RoomChdPax,
                        //RoomCreateDate = room.CreateDate,
                        //RoomDesc = room.RoomDesc,
                        //RoomHotelCode = room.HotelCode,
                        //RoomHotelSeasonBegin = room.HotelSeasonBegin,
                        //RoomHotelSeasonEnd = room.HotelSeasonEnd,
                        //RoomNotCountExcludingAccomDate = room.NotCountExcludingAccomDate,
                        //RoomOption = room.Option,
                        //RoomPax = room.RoomPax,
                        //RoomPrice = room.Price,
                        //RoomPriceType = room.PriceType,
                        //RoomRecID = room.RecID,
                        //RoomReleaseDay = room.ReleaseDay,
                        //RoomSPODefinit = room.SPODefinit,
                        //RoomSpoNoApply = room.SpoNoApply,
                        //RoomSPOPrices = room.SPOPrices,
                        //RoomSyncDate = room.SyncDate,
                        //RoomType = room.RoomType,
                        //RoomTypeDesc = room.RoomTypeDesc,
                        //RoomWeekendPercent = room.WeekendPercent,
                        //RoomWeekendPrice = room.WeekendPrice
                    });
                }
            }

            return retValue;
        }
        public async Task<List<SearchFligtAndRoomResponse>> SearchFlightAndRoomMockAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        {
            int flightAdultCount = searchFlightAndRoomRequest.FlightAdult;
            int flightChildCount = 0;
            int flightInfantCount = 0;
            List<SearchFligtAndRoomResponse> retValue = new();

            // Define child counts and ages for Flight seach
            if (searchFlightAndRoomRequest.RoomChildAges != null)
            {
                foreach (var man in searchFlightAndRoomRequest.RoomChildAges)
                {
                    if (man <= 2)
                    { flightInfantCount++; }
                    else if (man > 2 && man <= 12)
                    { flightChildCount++; }
                    else if (man > 12)
                    { flightAdultCount++; }
                }
            }

            SearchFlightResult searchFlightRequest = new()
            {
                FlightOneId = searchFlightAndRoomRequest.FlightOneId,
                FlightTwoId = searchFlightAndRoomRequest.FlightTwoId,
                StartDate = searchFlightAndRoomRequest.FlightStartDate,
                ReturnedDate = searchFlightAndRoomRequest.FlightReturnedDate,
                Adult = flightAdultCount,   // searchFlightAndRoomRequest.FlightAdult,
                Child = flightChildCount,   // searchFlightAndRoomRequest.FlightChild,
                Infant = flightInfantCount  // searchFlightAndRoomRequest.FlightInfant
            };

            // Get FLIGHTS
            List<FinalFlightSearchResponse> resultFlightSearch = await _priceBlockManager.GetFligthSearchResultMockAsync(searchFlightRequest);

            // Get ROOMS
            var resultRoomSearch = await _contractExportRepository.SearchRoomMockAsync(searchFlightAndRoomRequest);


            // Combine results
            foreach (var flight in resultFlightSearch)
            {
                foreach (var room in resultRoomSearch)
                {
                    retValue.Add(new SearchFligtAndRoomResponse()
                    {
                        flightSearchResponse = flight,
                        roomSearchResponse = room,
                        flightAndRoomTotalPrice = Math.Ceiling((double)flight.TotalPrice + (double)room.Price)

                    });
                }
            }

            return retValue;
        }
        public async Task<List<SearchFligtAndRoomLowestPricesResponse>> SearchFlightAndRoomLowestPricesAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        {
            int flightAdultCount = searchFlightAndRoomRequest.FlightAdult;
            int flightChildCount = 0;
            int flightInfantCount = 0;
            List<SearchFligtAndRoomLowestPricesResponse> retValue = new();

            // Define child counts and ages for Flight seach
            if (searchFlightAndRoomRequest.RoomChildAges != null)
            {
                foreach (var man in searchFlightAndRoomRequest.RoomChildAges)
                {
                    if (man <= 2)
                    { flightInfantCount++; }
                    else if (man > 2 && man <= 12)
                    { flightChildCount++; }
                    else if (man > 12)
                    { flightAdultCount++; }
                }
            }

            SearchFlightResult searchFlightRequest = new()
            {
                FlightOneId = searchFlightAndRoomRequest.FlightOneId,
                FlightTwoId = searchFlightAndRoomRequest.FlightTwoId,
                StartDate = searchFlightAndRoomRequest.FlightStartDate,
                ReturnedDate = searchFlightAndRoomRequest.FlightReturnedDate,
                Adult = flightAdultCount,   // searchFlightAndRoomRequest.FlightAdult,
                Child = flightChildCount,   // searchFlightAndRoomRequest.FlightChild,
                Infant = flightInfantCount  // searchFlightAndRoomRequest.FlightInfant
            };

            // Get FLIGHTS
            List<FinalFlightSearchResponse> resultFlightSearch = await _priceBlockManager.GetFligthSearchResultAsync(searchFlightRequest);
            //List<FinalFlightSearchResponse> resultFlightSearch = await _priceBlockManager.GetFligthSearchResultMockAsync(searchFlightRequest);      // MOCK

            SearchRoomRequest searchRoomRequest = new()
            {
                AccomodationDateFrom = searchFlightAndRoomRequest.RoomAccomodationDateFrom,
                AccomodationDateTo = searchFlightAndRoomRequest.RoomAccomodationDateTo,
                Board = searchFlightAndRoomRequest.Board,
                AdultCount = searchFlightAndRoomRequest.RoomAdultCount,
                ChildCount = searchFlightAndRoomRequest.RoomChildCount,
                ChildAges = searchFlightAndRoomRequest.RoomChildAges,
                PageNumber = searchFlightAndRoomRequest.PageNumber,
                PageSize = searchFlightAndRoomRequest.PageSize,
                LateCheckout = searchFlightAndRoomRequest.LateCheckout,
                HotelCodes = searchFlightAndRoomRequest.HotelCodes,
                HotelCategoryIds = searchFlightAndRoomRequest.HotelCategoryIds,
                HotelCountryIds = searchFlightAndRoomRequest.HotelCountryIds,
                HotelCityIds = searchFlightAndRoomRequest.HotelCityIds,
                TotalPriceMin = searchFlightAndRoomRequest.TotalPriceMin,
                TotalPriceMax = searchFlightAndRoomRequest.TotalPriceMax
            };

            // Get ROOMS
            var resultRoomSearch = await _contractExportRepository.SearchRoomLowestPricesAsync(searchRoomRequest);

            resultFlightSearch.ForEach(flight => flight.TotalPrice = Math.Ceiling((double)flight.TotalPrice));
            resultRoomSearch.ForEach(room => room.Price = Math.Ceiling((double)room.Price));

            // Combine results
            foreach (var flight in resultFlightSearch)
            {
                //flight.TotalPrice = Math.Ceiling((double)flight.TotalPrice);
                foreach (var room in resultRoomSearch)
                {
                    //room.Price = Math.Ceiling((double)room.Price);
                    retValue.Add(new SearchFligtAndRoomLowestPricesResponse()
                    {
                        flightSearchResponse = flight,
                        roomSearchResponse = room,
                        flightAndRoomTotalPrice = Math.Ceiling((double)flight.TotalPrice + (double)room.Price)
                    });
                }
            }

            return retValue;
        }
        public async Task<List<SearchFligtAndRoomResponse>> SearchFlightAndRoomLowestPricesMockAsync(SearchFligtAndRoomRequest searchFlightAndRoomRequest)
        {
            int flightAdultCount = searchFlightAndRoomRequest.FlightAdult;
            int flightChildCount = 0;
            int flightInfantCount = 0;
            List<SearchFligtAndRoomResponse> retValue = new();

            // Define child counts and ages for Flight seach
            if (searchFlightAndRoomRequest.RoomChildAges != null)
            {
                foreach (var man in searchFlightAndRoomRequest.RoomChildAges)
                {
                    if (man <= 2)
                    { flightInfantCount++; }
                    else if (man > 2 && man <= 12)
                    { flightChildCount++; }
                    else if (man > 12)
                    { flightAdultCount++; }
                }
            }

            SearchFlightResult searchFlightRequest = new()
            {
                FlightOneId = searchFlightAndRoomRequest.FlightOneId,
                FlightTwoId = searchFlightAndRoomRequest.FlightTwoId,
                StartDate = searchFlightAndRoomRequest.FlightStartDate,
                ReturnedDate = searchFlightAndRoomRequest.FlightReturnedDate,
                Adult = flightAdultCount,   // searchFlightAndRoomRequest.FlightAdult,
                Child = flightChildCount,   // searchFlightAndRoomRequest.FlightChild,
                Infant = flightInfantCount  // searchFlightAndRoomRequest.FlightInfant
            };

            // Get FLIGHTS
            List<FinalFlightSearchResponse> resultFlightSearch = await _priceBlockManager.GetFligthSearchResultMockAsync(searchFlightRequest);      // MOCK

            // Get ROOMS
            //var resultRoomSearch = await _contractExportRepository.SearchRoomAsync(searchRoomRequest);
            var resultRoomSearch = await _contractExportRepository.SearchRoomLowestPricesMockAsync(searchFlightAndRoomRequest);                     // MOCK


            // Combine results
            foreach (var flight in resultFlightSearch)
            {
                foreach (var room in resultRoomSearch)
                {
                    retValue.Add(new SearchFligtAndRoomResponse()
                    {
                        flightSearchResponse = flight,
                        roomSearchResponse = room,
                        flightAndRoomTotalPrice = flight.TotalPrice + room.Price
                    });
                }
            }

            return retValue;
        }
    }

}
