using AutoMapper;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Tnef;
using System;
using System.Collections.Generic;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class BookedFlightAndHotelManager : IBookedFlightAndHotelManager
    {
        protected readonly MaratukDbContext _dbContext;
        private readonly IBookedFlightRepository _bookedFlightRepository;
        private readonly IBookedHotelRepository _bookedHotelRepository;
        private readonly IHotelRepository _hotelRepository;
        //private readonly IContractExportRepository _contractExportRepository;
        //private readonly ITransactionRepository _transactionRepository;
        private readonly ICountryManager _countryManager;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public BookedFlightAndHotelManager(MaratukDbContext dbContext,
                                            IBookedFlightRepository bookedFlightRepository,
                                            IBookedHotelRepository bookedHotelRepository,
                                            IHotelRepository hotelRepository,
                                            //IContractExportRepository contractExportRepository,
                                            //ITransactionRepository transactionRepository,
                                            ICountryManager countryManager,
                                            IUserRepository userRepository,
                                            ICurrencyRatesRepository currencyRatesRepository,
                                            IFlightRepository flightRepository,
                                            IMapper mapper
                                            )
        {
            _dbContext = dbContext;
            _bookedFlightRepository = bookedFlightRepository;
            _bookedHotelRepository = bookedHotelRepository;
            _hotelRepository = hotelRepository;
            //_contractExportRepository = contractExportRepository;
            //_transactionRepository = transactionRepository;
            _countryManager = countryManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _currencyRatesRepository = currencyRatesRepository;
            _flightRepository = flightRepository;
        }

        public async Task<string> AddBookedFlightAndHotelAsync(BookedFlightAndHotel bookedFlightAndHotel)
        {
            string orderNumber = "";
            try
            {
                //var bookedFlightOrderNumber = await _bookedFlightManager.AddBookedFlightAsync(addBookedFlights);

                var USDRate = _currencyRatesRepository.GetAsync(1).Result.OfficialRate;
                string agentName = string.Empty;
                string companyName = string.Empty;
                string agentPhone = string.Empty;
                string agentEmail = string.Empty;
                List<string> listOfArrivals = new List<string>();
                List<string> listOfGuests = new List<string>();
                string Fligthname1 = string.Empty;
                string FligthNumber1 = string.Empty;
                string Fligthname2 = string.Empty;
                string FligthNumber2 = string.Empty;
                string TotalCurrency = string.Empty;
                string totalPay = string.Empty;
                double totalPayFlight = 0;
                string maratukAgentEmail = string.Empty;
                //List<BookedHotelGuest> bookedHotelGuests = new();
                int guestsCount = 0;

                DateTime tourStartDate = DateTime.MinValue;
                DateTime? tourEndDate = null;
                int countryId = 0;
                string? hotelName = string.Empty;
                string? hotelCountry = string.Empty;
                string? hotelCity = string.Empty;

                // *** Flight part
                orderNumber = "RAN" + RandomNumberGenerators.GenerateRandomNumber(10);

                List<AddBookedFlight> addBookedFlight = bookedFlightAndHotel.BookedFlights;

                //await _transactionRepository.BeginTransAsync();                                             // Begin transaction

                foreach (var bookedFlight in addBookedFlight)
                {
                    BookedFlight booked = new BookedFlight();
                    booked.OrderNumber = orderNumber;
                    booked.Name = bookedFlight.Name;
                    booked.Surname = bookedFlight.Surname;
                    booked.PhoneNumber = bookedFlight.PhoneNumber;
                    booked.BirthDay = bookedFlight.BirthDay;
                    booked.Email = bookedFlight.Email;
                    booked.Passport = bookedFlight.Passport;
                    booked.AgentId = bookedFlight.AgentId;
                    booked.DateOfOrder = DateTime.Now;
                    booked.ToureTypeId = "Flight";
                    booked.TotalPrice = bookedFlight.TotalPrice + bookedFlightAndHotel.Price;
                    booked.Rate = bookedFlight.Rate;
                    booked.TotalPriceAmd = (USDRate * bookedFlight.TotalPrice) + (USDRate * bookedFlightAndHotel.Price);
                    booked.PassengersCount = bookedFlight.PassengersCount;
                    booked.TourStartDate = bookedFlight.TourStartDate;
                    booked.TourEndDate = bookedFlight.TourEndDate;
                    booked.MaratukAgentId = bookedFlight.MaratukAgentId;
                    booked.CountryId = bookedFlight.CountryId;
                    booked.StartFlightId = bookedFlight.StartFlightId;
                    booked.EndFlightId = bookedFlight.EndFlightId;
                    booked.PasportExpiryDate = bookedFlight.PasportExpiryDate;
                    booked.GenderId = bookedFlight.GenderId;
                    booked.Dept = bookedFlight.TotalPrice;
                    booked.HotelId = bookedFlightAndHotel.HotelId;

                    var fligth = await _flightRepository.GetFlightByIdAsync(booked.StartFlightId);
                    Fligthname1 = fligth.Name;
                    FligthNumber1 = fligth.FlightValue;
                    //totalPay = booked.TotalPrice.ToString();
                    totalPayFlight = booked.TotalPrice;

                    if (booked.EndFlightId != null)
                    {
                        var fligth2 = await _flightRepository.GetFlightByIdAsync(booked.EndFlightId);
                        Fligthname2 = fligth.Name;
                        FligthNumber2 = fligth.FlightValue;
                    }

                    var agent = await _userRepository.GetAgencyUsersByIdAsync(booked.AgentId);

                    if (agent != null)
                    {
                        agentName = agent.FullName;
                        agentEmail = agent.Email;
                        agentPhone = agent.PhoneNumber1;
                    }

                    listOfArrivals.Add(booked.Name + " " + booked.Surname);
                    var maratukAgent = await _userRepository.GetUserByIdAsync(booked.MaratukAgentId);
                    if (maratukAgent != null)
                    {
                        maratukAgentEmail = maratukAgent.Email;
                    }
                    await _bookedFlightRepository.CreateBookedFlightAsync(booked);

                    // Variables to use for Hotel
                    guestsCount++;
                    tourStartDate = booked.TourStartDate;
                    tourEndDate = booked.TourEndDate;
                    countryId = booked.CountryId;

                    listOfGuests.Add(booked.Name + " " + booked.Surname);
                }

                // *** Hotel part
                BookedHotel bookedHotel = new()
                {
                    OrderNumber = orderNumber,
                    Room = bookedFlightAndHotel.Room,
                    RoomCode = bookedFlightAndHotel.RoomType,
                    HotelId = bookedFlightAndHotel.HotelId,
                    HotelCode = bookedFlightAndHotel.HotelCode,
                    SejourRateId = bookedFlightAndHotel.SejourRateId,
                    CountryId = countryId,
                    ToureTypeId = "Hotel",
                    TotalPrice = bookedFlightAndHotel.Price,
                    TotalPriceAmd = USDRate * bookedFlightAndHotel.Price,
                    GuestsCount = guestsCount,
                    TourStartDate = tourStartDate,
                    TourEndDate = tourEndDate,
                    Dept = bookedFlightAndHotel.Price,
                    Board = bookedFlightAndHotel.Board,
                    BoardDesc = bookedFlightAndHotel.BoardDesc
                };

                await _bookedHotelRepository.CreateBookedHotelAsync(bookedHotel);

                var hotel = await _hotelRepository.GetHotelByCodeAsync(bookedFlightAndHotel.HotelCode);

                if (hotel != null && hotel.hotel != null)
                {
                    hotelName = hotel.hotel.Name;
                    hotelCountry = hotel.hotelCountryNameEng;
                    hotelCity = hotel.hotelCityNameEng;
                }

                totalPay = (totalPayFlight + bookedHotel.TotalPrice).ToString();

                //await _transactionRepository.CommitTransAsync();                                            // Commit transaction

                string listOfArrivalsString = string.Join(", ", listOfArrivals);
                string date = DateTime.Now.ToString();
                string textBody = $@"
                                    {orderNumber}
                                    Agent: {companyName} 
                                    Creator: {agentName}
                                    Phone Number: {agentPhone}
                                    Email: {agentEmail}
                                    Full list of arrivals: {listOfArrivalsString}
                                    {Fligthname1} / {FligthNumber1} / 08:15-09:15
                                    {Fligthname2} / {FligthNumber2} / 22:15-23:15
                                    Hotel: {hotelName}
                                    City/Country: {hotelCity} / {hotelCountry}
                                    Country: {hotelCountry}
                                    Total payable: {totalPay} 
                                    Date of sale: {date}";

                MailService.SendEmail(maratukAgentEmail, $"New Request {orderNumber}", textBody);
            }
            catch (Exception)
            {
                //await _transactionRepository.RollbackTransAsync();        // Rollback transaction
                throw;
            }

            return orderNumber;
        }

        public async Task<List<BookedHotelResponse>> GetBookedFlightsAndHotelsAsync(int Itn)
        {
            return new List<BookedHotelResponse>();
        }
    }
}
