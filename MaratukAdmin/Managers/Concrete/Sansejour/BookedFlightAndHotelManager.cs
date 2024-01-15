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
        private readonly IContractExportRepository _contractExportRepository;
        private readonly ICountryManager _countryManager;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public BookedFlightAndHotelManager(MaratukDbContext dbContext,
                                            IBookedFlightRepository bookedFlightRepository,
                                            IBookedHotelRepository bookedHotelRepository,
                                            IContractExportRepository contractExportRepository,
                                            ICountryManager countryManager,
                                            IUserRepository userRepository,
                                            ICurrencyRatesRepository currencyRatesRepository,
                                            IFlightRepository flightRepository,
                                            IMapper mapper,
                                            ITransactionRepository transactionRepository)
        {
            _dbContext = dbContext;
            _bookedFlightRepository = bookedFlightRepository;
            _bookedHotelRepository = bookedHotelRepository;
            _contractExportRepository = contractExportRepository;
            _countryManager = countryManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _currencyRatesRepository = currencyRatesRepository;
            _flightRepository = flightRepository;
            _transactionRepository = transactionRepository;
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
                string maratukAgentEmail = string.Empty;
                List<BookedHotelGuest> bookedHotelGuests = new();
                int guestsCount = 0;

                int agentId = 0;
                double totalPrice = 0;
                string rate = string.Empty;
                double totalPriceAmd = 0;
                DateTime tourStartDate = DateTime.MinValue;
                DateTime? tourEndDate = null;
                int maratukAgentId = 0;
                double? dept = 0;
                int countryId = 0;


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
                    booked.TotalPrice = bookedFlight.TotalPrice;
                    booked.Rate = bookedFlight.Rate;
                    booked.TotalPriceAmd = USDRate * bookedFlight.TotalPrice;
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

                    var fligth = await _flightRepository.GetFlightByIdAsync(booked.StartFlightId);
                    Fligthname1 = fligth.Name;
                    FligthNumber1 = fligth.FlightValue;
                    totalPay = booked.TotalPrice.ToString();

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

                    // Hotel part
                    guestsCount++;
                    agentId = booked.AgentId;
                    totalPrice = booked.TotalPrice;
                    rate = booked.Rate;
                    totalPriceAmd = booked.TotalPriceAmd;
                    tourStartDate = booked.TourStartDate;
                    tourEndDate = booked.TourEndDate;
                    maratukAgentId = booked.MaratukAgentId;
                    dept = booked.Dept;
                    countryId = booked.CountryId;

                    BookedHotelGuest bookedGuest = new()
                    {
                        OrderNumber = booked.OrderNumber,
                        //IsAdult = hotelGuest.IsAdult,
                        GenderId = booked.GenderId,
                        Name = booked.Name,
                        Surname = booked.Surname,
                        PhoneNumber = booked.PhoneNumber,
                        BirthDay = booked.BirthDay,
                        Email = booked.Email,
                        Passport = booked.Passport,
                        PassportExpiryDate = booked.PasportExpiryDate
                    };

                    bookedHotelGuests.Add(bookedGuest);
                    listOfGuests.Add(booked.Name + " " + booked.Surname);
                    // *** End of Hotel part
                }
                // *** End of Flight part

                // *** Hotel part
                int roomId = bookedFlightAndHotel.BookedRoomId;

                var sejourRate = await _contractExportRepository.GetSyncSejourRateByIdAsync(roomId);

                if (sejourRate != null)
                {
                    BookedHotel bookedHotel = new()
                    {
                        RoomId = roomId,
                        OrderNumber = orderNumber,
                        AgentId = agentId,
                        CountryId = countryId,
                        DateOfOrder = DateTime.Now,
                        ToureTypeId = "Hotel",
                        HotelCode = sejourRate.HotelCode,
                        TotalPrice = totalPrice,
                        Rate = rate,
                        TotalPriceAmd = totalPriceAmd,
                        GuestsCount = guestsCount,
                        TourStartDate = tourStartDate,
                        TourEndDate = tourEndDate,
                        MaratukAgentId = maratukAgentId,
                        Dept = totalPrice,
                        //DeadLine,
                        //OrderStatusId
                        //Paid
                        RoomCode = sejourRate.Room
                    };

                    await _bookedHotelRepository.CreateBookedHotelAsync(bookedHotel, bookedHotelGuests);
                }
                // *** End of Hotel part

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

        public async Task<List<BookedHotelResponse>> GetBookedHotelAsync(int Itn)
        {
            BookedFlightResponseFinal responseFinal = new BookedFlightResponseFinal();

            var USDRate = _currencyRatesRepository.GetAsync(1).Result.OfficialRate;


            var response = await _userRepository.GetAgencyUsersAsync(Itn);

            //var listBookedFlights = await _bookedFlightRepository.GetAllBookedFlightAsync(response);
            var listBookedHotels = await _bookedHotelRepository.GetAllBookedHotelsAsync(response);

            return listBookedHotels;

            ////double totalDeptUsd = listBookedFlights.Sum(bf => bf.Dept ?? 0);
            //double totalDeptUsd = listBookedHotels.Sum(bf => bf.BookedHotel.Dept ?? 0);

            //var groupedBookedHotels = listBookedHotels.GroupBy(flight => flight.BookedHotel.OrderNumber).ToList();

            ////var bookedFlightResponses = new List<BookedFlightResponse>();
            //var bookedFlightResponses = new BookedHotelResponse();

            //foreach (var group in groupedBookedHotels)
            //{
            //    //var bookedUsers = group.Select(hotel => new BookedHotelGuest
            //    var bookedUsers = group.Select(hotel => new BookedHotelGuest
            //    {
            //        Name = hotel.BookedHotelGuests.Name,
            //        //Surname = hotel.Surname,
            //        //PhoneNumber = hotel.PhoneNumber,
            //        //BirthDay = hotel.BirthDay,
            //        //Email = hotel.Email,
            //        //Passport = hotel.Passport,
            //        //PasportExpiryDate = hotel.PasportExpiryDate,
            //        //GenderName = (hotel.GenderId == 1) ? "Male" : "Female"
            //        // Map other properties as needed
            //    }).ToList();

            //    var firstFlightInGroup = group.First(); // You can take any flight from the group to extract common properties
            //    var bookedFlightResponse = new BookedFlightResponse
            //    {
            //        bookedUsers = bookedUsers,
            //        Id = firstFlightInGroup.Id,
            //        OrderNumber = firstFlightInGroup.OrderNumber,
            //        DateOfOrder = firstFlightInGroup.DateOfOrder,
            //        ToureTypeId = firstFlightInGroup.ToureTypeId,
            //        HotelId = firstFlightInGroup.HotelId,
            //        TicketNumber = firstFlightInGroup.TicketNumber,
            //        OrderStatusId = firstFlightInGroup.OrderStatusId,
            //        TotalPrice = firstFlightInGroup.TotalPrice,
            //        Rate = firstFlightInGroup.Rate,
            //        AgentId = firstFlightInGroup.AgentId,//add agentName
            //        AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
            //        TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
            //        PassengersCount = firstFlightInGroup.PassengersCount,
            //        TourStartDate = firstFlightInGroup.TourStartDate,
            //        TourEndDate = firstFlightInGroup.TourEndDate,
            //        DeadLine = firstFlightInGroup.DeadLine,
            //        Paid = firstFlightInGroup.Paid,
            //        MaratukAgentId = firstFlightInGroup.MaratukAgentId,
            //        MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
            //        CountryId = firstFlightInGroup.CountryId,
            //        CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
            //        Dept = firstFlightInGroup.Dept,
            //        StartFlightId = firstFlightInGroup.StartFlightId,
            //        EndFlightId = firstFlightInGroup.EndFlightId,
            //    };

            //    bookedFlightResponses.Add(bookedFlightResponse);
            //}

            //responseFinal.bookedFlightResponses = bookedFlightResponses;
            //responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            //responseFinal.DeptEUR = 0;

            //return responseFinal;

        }
    }
}
