using AutoMapper;
using Azure;
using Bogus.DataSets;
using MailKit;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class HotelManager : IHotelManager
    {
        private readonly IMainRepository<Hotel> _mainRepository;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly IHttpRequestManager _httpRequestManager;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDistributedCache _cache;

        public HotelManager(IMainRepository<Hotel> mainRepository,
                            IMapper mapper,
                            IHotelRepository hotelRepository,
                            IHttpRequestManager httpRequestManager,
                            ITransactionRepository transactionRepository,
                            IDistributedCache cache,
                            IFakeDataGenerationManager fakeDataGenerationManager

            )
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _httpRequestManager = httpRequestManager;
            _transactionRepository = transactionRepository;
            _cache = cache;
        }
        //public async Task<List<HotelResponse>> GetAllHotelsAsync()
        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            var result = await _mainRepository.GetAllAsync();

            //var hotelsResponse = new List<HotelResponse>();


            //foreach (var hotel in result)
            //{
            //    var hotelResponse = new HotelResponse()
            //    {
            //        Id = hotel.Id,
            //        Name = hotel.Name,
            //        Code = hotel.Code,
            //        Country = hotel.Country,
            //        City = hotel.City
            //        //DepartureCountry = _countryManager.GetCountryNameByIdAsync(flight.DepartureCountryId).Result.NameENG,
            //        //DestinationCountry = _countryManager.GetCountryNameByIdAsync(flight.DestinationCountryId).Result.NameENG,
            //        //FlightValue = flight.FlightValue,
            //    };
            //    hotelsResponse.Add(hotelResponse);
            //}

            //return hotelsResponse;
            return result;
        }

        //public async Task<HotelResponse> GetHotelByIdAsync(int id)
        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            var entity = await _mainRepository.GetAsync(id);
            //var entity = await _mainRepository.GetAsync(id);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            //var hotelResponse = new HotelResponse()
            //{
            //    Id = entity.Id,
            //    Code = entity.Code,
            //    Country = entity.Country,
            //    City = entity.City,
            //    Name = entity.Name
            //};

            //return hotelResponse;
            return entity;

        }

        public async Task<HotelResponseModel> GetHotelByCodeAsync(string code)
        {
            // ***
            //var query = _dbContext.SyncSejourRate
            //.Join(distinctAccomodationDescriptions,
            //rate => rate.AccmdMenTypeCode,
            //accomodationDescription => accomodationDescription.Code,
            //(rate, accomodationDescription) => new { Rate = rate, AccomodationDescription = accomodationDescription })
            //.Where(joinResult =>
            //joinResult.Rate.SyncDate == exportDate &&
            //joinResult.Rate.RoomPax == roomPax &&
            //joinResult.Rate.RoomAdlPax == adultPax &&
            //joinResult.Rate.RoomChdPax == childPax &&
            //joinResult.Rate.AccomodationPeriodBegin >= accomodationDateFrom &&
            //joinResult.Rate.AccomodationPeriodEnd <= accomodationDateTo)
            ////.Select(joinResult => joinResult.Rate);
            //.OrderBy(joinResult => joinResult.Rate.Id)
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            //.Select(joinResult => joinResult.Rate);


            //return await query.ToListAsync();

            //public async Task<FlightEditResponse> GetFlightByIdAsync(int id)
            //{
            //    var entity = await _mainRepository.GetAsync(id, "Schedules");
            //    if (entity == null)
            //    {
            //        throw new ApiBaseException(StatusCodes.Status404NotFound);
            //    }

            //    var flightEditResponse = new FlightEditResponse();
            //    var sheduledEdit = new List<ScheduleEditResponse>();

            //    flightEditResponse.Name = entity.Name;
            //    flightEditResponse.Id = entity.Id;

            //    flightEditResponse.DepartureCountryId = entity.DepartureCountryId;
            //    flightEditResponse.DepartureCityId = entity.DepartureCityId;
            //    flightEditResponse.DepartureAirportId = entity.DepartureAirportId;


            //    flightEditResponse.DestinationCountryId = entity.DestinationCountryId;
            //    flightEditResponse.DestinationCityId = entity.DestinationCityId;
            //    flightEditResponse.DestinationAirportId = entity.DestinationAirportId;

            //    flightEditResponse.FlightValue = entity.FlightValue;
            //    flightEditResponse.AirlineId = entity.AirlineId;
            //    flightEditResponse.AircraftId = entity.AircraftId;
            //    flightEditResponse.DurationHours = entity.DurationHours;
            //    flightEditResponse.DurationMinutes = entity.DurationMinutes;


            //    if (entity.Schedules != null)
            //    {
            //        foreach (var shedul in entity.Schedules)
            //        {
            //            var schedule = new ScheduleEditResponse()
            //            {
            //                FlightStartDate = shedul.FlightStartDate,
            //                FlightEndDate = shedul.FlightEndDate,
            //                DepartureTime = shedul.DepartureTime,
            //                ArrivalTime = shedul.ArrivalTime,
            //                DayOfWeek = shedul.DayOfWeek.Split(',')
            //            };



            //            sheduledEdit.Add(schedule);

            //        }
            //    }

            //    flightEditResponse.schedules = sheduledEdit;

            //    return flightEditResponse;

            //}
            // ***
            var entity = await _hotelRepository.GetHotelByCodeAsync(code);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;
        }

        public async Task<List<Hotel>?> GetHotelsByCountryIdAndCityIdAsync(List<int>? countryIds = null, List<int>? cityIds = null)
        {
            var entity = await _hotelRepository.GetHotelsByCountryIdAndCityIdAsync(countryIds, cityIds);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;
        }
        public async Task<List<HotelResponseModel>?> GetHotelsByCountryIdAndCityIdAsync(int? countryId = null, int? cityId = null)
        {
            var entity = await _hotelRepository.GetHotelsByCountryIdAndCityIdAsync(countryId, cityId);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;
        }

        public async Task<HotelResponseModel> GetHotelByCodeMockAsync(string code)
        {
            var entity = await _hotelRepository.GetHoteByCodeMockAsync(code);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound);
            }

            return entity;

        }
        public async Task<bool> RefreshHotelList()
        {
            //string databaseName = "DATA2020";
            //string userName = "MARTUK";
            //string password = "MAR2023";
            //string language = "en";

            try
            {
                #region *** LOGIN ***
                await _httpRequestManager.LoginAsync();

                //string soapEnvelopeLogin = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:san=""http://www.sansejour.com/"">
                //                       <soapenv:Header/>
                //                       <soapenv:Body>
                //                          <san:Login>
                //                             <san:databaseName>{databaseName}</san:databaseName>
                //                             <san:userName>{userName}</san:userName>
                //                             <san:password>{password}</san:password>
                //                             <san:Language>{language}</san:Language>
                //                          </san:Login>
                //                       </soapenv:Body>
                //                    </soapenv:Envelope>";

                //var loginRequest = new HttpRequestMessage(HttpMethod.Post, "http://196.219.84.44/sws/Authentication.asmx");
                //loginRequest.Headers.Add("SOAPAction", "http://www.sansejour.com/Login");
                //loginRequest.Content = new StringContent(soapEnvelopeLogin, Encoding.UTF8, "text/xml");

                //var respLogin = await _httpRequestManager.SendAsync(loginRequest);

                //var respLoginContent = await respLogin.Content.ReadAsStringAsync();

                //var xmlSerializer = new XmlSerializer(typeof(Dto.Response.Sansejour.Envelope));

                //// Deserialize XML to object
                //Dto.Response.Sansejour.Envelope response;
                //using (var reader = new StringReader(respLoginContent))
                //{
                //    response = (Dto.Response.Sansejour.Envelope)xmlSerializer.Deserialize(reader);
                //}
                ////MaratukLoginResponse loginResult = JsonConvert.DeserializeObject<MaratukLoginResponse>(respLoginContent);

                //string token = response.Body.LoginResponse.LoginResult.AuthKey;
                #endregion


                #region *** GetHotels ***
                var hotelsSansejourList = await _httpRequestManager.GetAllHotelsSansejourAsync();

                //string token = await _cache.GetStringAsync("token");

                //string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                //                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:san=""http://www.sansejour.com/"">
                //                    <soapenv:Header/>
                //                    <soapenv:Body>
                //                        <san:GetHotels>
                //                            <san:token>{token}</san:token>
                //                        </san:GetHotels>
                //                    </soapenv:Body>
                //                </soapenv:Envelope>";

                //var request = new HttpRequestMessage(HttpMethod.Post, "http://196.219.84.44/sws/hotel.asmx");
                //request.Headers.Add("SOAPAction", "http://www.sansejour.com/GetHotels");
                //request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

                //var resp = await _httpRequestManager.SendAsync(request);

                //var responseContent = await resp.Content.ReadAsStringAsync();
                #endregion

                #region *** Delete existing data and insert new ***
                if (hotelsSansejourList != null)
                {
                    //await _transactionRepository.BeginTransAsync();                                             // Begin transaction
                    //await _hotelRepository.EraseHotelListAsync();                         // commented on 23.01.24 to avoid Hotel dictionary data deletion

                    List<Hotel> hotelsToInsert = hotelsSansejourList.Select(hotelSansejour => new Hotel
                    {
                        Code = hotelSansejour.Code,
                        Name = hotelSansejour.Name
                        //Country = hotelSansejour.Country,
                        //City = hotelSansejour.City
                    }).ToList();

                    // Get existing Hotels list
                    var existingEntities = await _hotelRepository.GetAllHotelsAsync();

                    if (existingEntities != null)
                    {
                        hotelsToInsert = hotelsToInsert.Except(existingEntities, new HotelComparer()).ToList();
                    }


                    await _hotelRepository.FillNewHotelsListAsync(hotelsToInsert);
                    //await _transactionRepository.CommitTransAsync();                                            // Commit transaction

                    //Hotel hotel = new Hotel();

                    //var entity = _mapper.Map<Hotel>(new Hotel());
                    //await _mainRepository.ResetTableIdentityAsync(entity);

                    //var tableName = _dbContext.Model.FindEntityType(typeof(Hotel)).GetTableName();
                    //await _transactionRepository.ResetIndentAsync(typeof(Hotel).GetTableName());
                    //await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('" + tableName + "', RESEED, 0)");

                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public async Task<Hotel> UpdateHotelAsync(UpdateHotelRequest updateHotelRequest)
        {
            Hotel result;
            try
            {
                Hotel hotelEntity = await _mainRepository.GetAsync(updateHotelRequest.hotel.Id);
                if (hotelEntity == null)
                {
                    throw new ApiBaseException(StatusCodes.Status404NotFound);
                }

                hotelEntity = updateHotelRequest.hotel;


                //var executionStrategy = _transactionRepository.CreateExecutionStrategy();
                //executionStrategy.Execute(
                //() =>
                //{
                //    _transactionRepository.BeginTransAsync();                                             // Begin transaction
                //    result = _mainRepository.UpdateAsync(updateHotelRequest.hotel).Result;
                //    _transactionRepository.CommitTransAsync();                                            // Commit transaction
                //});

                result = await _mainRepository.UpdateAsync(hotelEntity);
                //result = await _mainRepository.UpdateAsync(updateHotelRequest.hotel);

            }
            catch (Exception)
            {
                result = new();
            }
            return result;
        }

        public async Task<Hotel> AddHotelAsync(AddHotelRequest hotelRequest)
        {
            //return await _hotelRepository.AddHotelAsync(hotelRequest);
            var entity = _mapper.Map<Hotel>(hotelRequest);
            await _mainRepository.AddAsync(entity);

            return entity;
        }


    }
}
