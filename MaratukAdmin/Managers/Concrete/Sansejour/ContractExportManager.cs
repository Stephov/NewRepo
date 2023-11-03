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

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class ContractExportManager : IContractExportManager
    {
        private readonly IMainRepository<SyncSejourContractExportView> _mainRepository;
        //private readonly IMapper _mapper;
        private readonly IContractExportRepository _contractExportRepository;
        private readonly IHttpRequestManager _httpRequestManager;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDistributedCache _cache;


        public ContractExportManager(IMainRepository<SyncSejourContractExportView> mainRepository,
                            //IMapper mapper,
                            IContractExportRepository contractExportRepository,
                            IHttpRequestManager httpRequestManager,
                            ITransactionRepository transactionRepository,
                            IDistributedCache cache

            )
        {
            _mainRepository = mainRepository;
            //_mapper = mapper;
            _contractExportRepository = contractExportRepository;
            _httpRequestManager = httpRequestManager;
            _transactionRepository = transactionRepository;
            _cache = cache;
        }
        public async Task<bool> GetSejourContractExportView(List<HotelSansejourResponse>? hotelsList = null)
        {
            bool retValue;
            bool contractExportViewRecorded = false;
            bool previousDataDeleted = false;
            int skippedHotels = 0;
            int processedHotels = 0;
            string dateString = "";
            string syncDateFormat = "";
            string processingHotelCode = "";
            DateTime syncDate = DateTime.MinValue;

            DateTime dateStartSession = DateTime.Now;
            DateTime dateStartLoop = DateTime.Now;
            TimeSpan elapsed;


            try
            {
                await _httpRequestManager.LoginAsync();
                string token = await _cache.GetStringAsync("token");

                hotelsList ??= await _httpRequestManager.GetAllHotelsSansejourAsync();

                hotelsList = hotelsList.Where(hotel => hotel != null).ToList();

                dateStartLoop = DateTime.Now;

                await _transactionRepository.BeginTransAsync();                                             // Begin transaction


                // *** Loop for HOTELS ***
                foreach (var hotel in hotelsList)
                {
                    dateStartLoop = DateTime.Now;
                    processingHotelCode = hotel.Code;
                    System.Diagnostics.Debug.WriteLine($"--- PROCESSING --- Hotel Code: {processingHotelCode}");

                    GetSejourContractExportViewRequestModel reqModel = new()
                    {
                        Token = token,
                        Season = "S23",
                        HotelCode = hotel.Code
                    };

                    SyncSejourContractExportViewResponse sejourContracts = await _httpRequestManager.GetSejourContractExportViewAsync(reqModel);

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
                        System.Diagnostics.Debug.WriteLine($"--- SKIP --- Hotel Code: {hotel.Code}");
                        continue;
                    }

                    dateString = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date;
                    syncDateFormat = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.DateFormat
                                    .Replace("mm", "MM");

                    dateString = "28/09/2023";
                    syncDateFormat = "dd/MM/yyyy";
                    syncDate = DateTime.ParseExact(dateString, syncDateFormat, CultureInfo.InvariantCulture);


                    // Delete previous data on this Date
                    if (!previousDataDeleted)
                    {
                        System.Diagnostics.Debug.WriteLine($"--- DELETING PREVIOUS DATA ---");
                        //var deleteResult = await _contractExportRepository.DeleteSyncedDataByDateAsync((DateTime)sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date);
                        var deleteResult = await _contractExportRepository.DeleteSyncedDataByDateAsync(syncDate);
                        if (!deleteResult)
                        { throw new Exception("Error deleting previous data"); }
                        previousDataDeleted = true;
                    }

                    string jsonSejourContracts = JsonConvert.SerializeObject(sejourContracts);

                    SyncSejourContractExportView contract = new()
                    {
                        Version = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Version,
                        //ExportDate = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date,
                        ExportDate = syncDate,
                        //DateFormat = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.DateFormat,
                        DateFormat = syncDateFormat,
                        SanBeginDate = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.SanBeginDate,
                        SanEndDate = sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.SanEndDate,
                    };

                    SyncSejourHotel syncSejourHotel = new()
                    {
                        //SyncDate = (DateTime)sejourContracts.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Sanbilgisayar.Date,
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
                                                    Price = (double)sejRate.Price,
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

                    //await _contractExportRepository.DeleteSyncSejourHotelsByDateAsync(syncSejourHotel.SyncDate, hotel.Code);
                    await _contractExportRepository.AddNewSejourHotelsAsync(syncSejourHotel);

                    //await _contractExportRepository.DeleteSyncSejourSpoAppOrdersByDateAsync(syncSejourHotel.SyncDate, hotel.Code);
                    await _contractExportRepository.AddNewSejourSpoAppOrdersAsync(sejourSpoAppOrders);

                    //await _contractExportRepository.DeleteSyncSejourSpecialOffersByDateAsync(syncSejourHotel.SyncDate, hotel.Code);
                    await _contractExportRepository.AddNewSejourSpecialOffersAsync(syncSejourSpecialOffers);

                    //await _contractExportRepository.DeleteSyncSejourRatesByDateAsync(syncSejourHotel.SyncDate, hotel.Code);
                    await _contractExportRepository.AddNewSejourRatesAsync(syncSejourRates);

                    //await _contractExportRepository.DeleteSyncSejourChildAgesByDateAsync(syncSejourHotel.SyncDate, hotel.Code);
                    //await _contractExportRepository.FillNewSejourChildAgesAsync(syncRateChildAges);

                    processedHotels++;
                    elapsed = DateTime.Now - dateStartLoop;
                    System.Diagnostics.Debug.WriteLine($"--- DONE --- Hotel Code: {hotel.Code}, elapsed: {elapsed.Seconds} seconds");

                    elapsed = DateTime.Now - dateStartSession;
                    System.Diagnostics.Debug.WriteLine($"--- TOTAL --- SKIPPED: {skippedHotels}, PROCESSED: {processedHotels}, Elapsed: {elapsed.Minutes} minutes, {elapsed.Seconds} seconds");
                }

                // *** GET possible AccomodationTypes from Rates 
                List<SyncSejourAccomodationType> accmdTypes = await _contractExportRepository.GetSyncSejourAccomodationTypesFromRatesBySyncDateAsync(syncDate);

                // ** Add gathered AccomodationTypes to DB
                await _contractExportRepository.AddNewSyncSejourAccomodationTypesAsync(accmdTypes);

                (int, int) counts = await _contractExportRepository.DescribeListOfAccomodationTypesAsync(accmdTypes);

                await _transactionRepository.CommitTransAsync();                                            // Commit transaction

                elapsed = DateTime.Now - dateStartSession;
                System.Diagnostics.Debug.WriteLine($"--- FINISHED --- SKIPPED: {skippedHotels}, PROCESSED: {processedHotels}, Elapsed: {elapsed.Minutes} minutes, {elapsed.Seconds} seconds");
                System.Diagnostics.Debug.WriteLine($"--- AVERAGE : {Math.Round((elapsed.TotalSeconds / (processedHotels + skippedHotels)), 2)} seconds");
                System.Diagnostics.Debug.WriteLine($"--- Total Accomodations: {counts.Item1}, Described: {counts.Item2}");


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

        public async Task<List<SyncSejourRate>> SearchRoomAsync(SearchRoomRequest searchRequest)
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

        public async Task<List<SyncSejourRate>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest)
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
    }

}
