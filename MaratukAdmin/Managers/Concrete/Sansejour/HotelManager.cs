﻿using AutoMapper;
using Azure;
using MailKit;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class HotelManager : IHotelManager
    {
        private readonly IMainRepository<Hotel> _mainRepository;
        //private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly IHttpRequestManager _httpRequestManager;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDistributedCache _cache;


        public HotelManager(IMainRepository<Hotel> mainRepository,
                            //IMapper mapper,
                            IHotelRepository hotelRepository,
                            IHttpRequestManager httpRequestManager,
                            ITransactionRepository transactionRepository,
                            IDistributedCache cache

            )
        {
            _mainRepository = mainRepository;
            //_mapper = mapper;
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
            var entity = await _mainRepository.GetAsync(id, "Hotels");

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

                if (hotelsSansejourList != null)
                {
                    await _transactionRepository.BeginTransAsync();                                             // Begin transaction
                    await _hotelRepository.EraseHotelListAsync();

                    List<Hotel> hotelsToInsert = hotelsSansejourList.Select(hotelSansejour => new Hotel
                    {
                        Code = hotelSansejour.Code,
                        Name = hotelSansejour.Name
                        //Country = hotelSansejour.Country,
                        //City = hotelSansejour.City
                    }).ToList();

                    await _hotelRepository.FillNewHotelsListAsync(hotelsToInsert);
                    await _transactionRepository.CommitTransAsync();                                            // Commit transaction

                    //Hotel hotel = new Hotel();

                    //var entity = _mapper.Map<Hotel>(new Hotel());
                    //await _mainRepository.ResetTableIdentityAsync(entity);

                    //var tableName = _dbContext.Model.FindEntityType(typeof(Hotel)).GetTableName();
                    //await _transactionRepository.ResetIndentAsync(typeof(Hotel).GetTableName());
                    //await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('" + tableName + "', RESEED, 0)");

                }
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }




    }
}
