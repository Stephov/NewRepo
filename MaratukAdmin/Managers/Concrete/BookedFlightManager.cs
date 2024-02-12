using AutoMapper;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Tnef;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MaratukAdmin.Managers.Concrete
{
    public class BookedFlightManager : IBookedFlightManager
    {

        private readonly IMainRepository<BookedFlight> _mainRepository;
        private readonly IBookedFlightRepository _bookedFlightRepository;
        private readonly IBookedHotelRepository _bookedHotelRepository;
        private readonly IHotelManager _hotelManager;
        private readonly ICountryManager _countryManager;
        private readonly IFlightRepository _flightRepository;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public BookedFlightManager(IMapper mapper, IBookedFlightRepository bookedFlightRepository,
            IBookedHotelRepository bookedHotelRepository,
            IHotelManager hotelManager,
            ICountryManager countryManager,
            IUserRepository userRepository,
            ICurrencyRatesRepository currencyRatesRepository,
            IFlightRepository flightRepository, IMainRepository<BookedFlight> mainRepository)
        {

            _bookedFlightRepository = bookedFlightRepository;
            _bookedHotelRepository = bookedHotelRepository;
            _countryManager = countryManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _currencyRatesRepository = currencyRatesRepository;
            _flightRepository = flightRepository;
            _mainRepository = mainRepository;
            _hotelManager = hotelManager;
        }

        public async Task<bool> AddBookedFlightAsync(List<AddBookedFlight> addBookedFlight)
        {
            try
            {
                var USDRate = _currencyRatesRepository.GetAsync(1).Result.OfficialRate;
                string agentName = string.Empty;
                string companyName = string.Empty;
                string agentPhone = string.Empty;
                string agentEmail = string.Empty;
                List<string> listOfArrivals = new List<string>();
                string Fligthname1 = string.Empty;
                string FligthNumber1 = string.Empty;
                string Fligthname2 = string.Empty;
                string FligthNumber2 = string.Empty;
                string TotalCurrency = string.Empty;
                string totalPay = string.Empty;
                string maratukAgentEmail = string.Empty;


                string orderNumber = "RAN" + RandomNumberGenerators.GenerateRandomNumber(10);
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

                }
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<BookedFlightResponseFinal> GetBookedFlightByAgentIdAsync(int id)
        {
            BookedFlightResponseFinal responseFinal = new BookedFlightResponseFinal();
            ///todo  add last actual currency
            var listBookedFlights = await _bookedFlightRepository.GetBookedFlightByAgentIdAsync(id);

            var groupedBookedFlights = listBookedFlights.GroupBy(flight => flight.OrderNumber).ToList();

            var bookedFlightResponses = new List<BookedFlightResponse>();
            double totalDeptUsd = 0;
            double totalDeptEur = 0;



            var groupedFlights = listBookedFlights
     .GroupBy(flight => new { flight.OrderNumber, flight.Rate })
     .Select(group => new
     {
         Currency = group.Key.Rate,
         TotalDept = group.Select(flight => flight.Dept ?? 0).Distinct().Sum()
     });


            foreach (var result in groupedFlights)
            {

                if (result.Currency == "USD")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptUsd += result.TotalDept;
                }

                if (result.Currency == "EUR")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptEur += result.TotalDept;
                }
            }

            foreach (var group in groupedBookedFlights)
            {
                var bookedUsers = group.Select(flight => new BookedUserInfo
                {
                    Name = flight.Name,
                    Surname = flight.Surname,
                    PhoneNumber = flight.PhoneNumber,
                    BirthDay = flight.BirthDay,
                    Email = flight.Email,
                    Passport = flight.Passport,
                    PasportExpiryDate = flight.PasportExpiryDate,
                    GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                }).ToList();

                var firstFlightInGroup = group.First();
                // You can take any flight from the group to extract common properties
                var bookedFlightResponse = new BookedFlightResponse
                {
                    bookedUsers = bookedUsers,
                    Id = firstFlightInGroup.Id,
                    OrderNumber = firstFlightInGroup.OrderNumber,
                    DateOfOrder = firstFlightInGroup.DateOfOrder,
                    ToureTypeId = firstFlightInGroup.ToureTypeId,
                    HotelId = firstFlightInGroup.HotelId,
                    TicketNumber = firstFlightInGroup.TicketNumber,
                    OrderStatusId = firstFlightInGroup.OrderStatusId,
                    TotalPrice = firstFlightInGroup.TotalPrice,
                    Rate = firstFlightInGroup.Rate,
                    AgentId = firstFlightInGroup.AgentId,//add agentName
                    AgentStatusId = firstFlightInGroup.AgentStatusId,
                    AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                    TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                    PassengersCount = firstFlightInGroup.PassengersCount,
                    TourStartDate = firstFlightInGroup.TourStartDate,
                    TourEndDate = firstFlightInGroup.TourEndDate,
                    DeadLine = firstFlightInGroup.DeadLine,
                    Paid = firstFlightInGroup.Paid,
                    MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                    MaratukAgentStatusId = firstFlightInGroup.MaratukAgentStatusId,
                    MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                    CountryId = firstFlightInGroup.CountryId,
                    CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                    Dept = firstFlightInGroup.Dept,
                    StartFlightId = firstFlightInGroup.StartFlightId,
                    EndFlightId = firstFlightInGroup.EndFlightId,
                    Comments = firstFlightInGroup.Comment,
                };

                bookedFlightResponses.Add(bookedFlightResponse);
            }


            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;

            return responseFinal;
        }


        public async Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightByMaratukAgentIdAsync(int maratukAgent, int pageNumber, int pageSize)
        {
            BookedFlightResponseFinalForMaratukAgent responseFinal = new BookedFlightResponseFinalForMaratukAgent();

            var listBookedFlightsAll = await _bookedFlightRepository.GetBookedFlightByMaratukAgentIdAsync(maratukAgent);

            var groupedBookedFlights = listBookedFlightsAll.GroupBy(flight => flight.OrderNumber).ToList();


            int distinctOrderNumbersCount = groupedBookedFlights.Count;




            var listBookedFlights = groupedBookedFlights
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();



            int totalPages = (int)Math.Ceiling((double)distinctOrderNumbersCount / pageSize);

            ///todo  add last actual currency


            var bookedFlightResponses = new List<BookedFlightResponseForMaratuk>();
            double totalDeptUsd = 0;
            double totalDeptEur = 0;



            var groupedFlights = listBookedFlightsAll
                                .GroupBy(flight => new { flight.OrderNumber, flight.Rate })
                                .Select(group => new
                                {
                                    Currency = group.Key.Rate,
                                    TotalDept = group.Select(flight => flight.Dept ?? 0).Distinct().Sum()
                                });


            foreach (var result in groupedFlights)
            {

                if (result.Currency == "USD")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptUsd += result.TotalDept;
                }

                if (result.Currency == "EUR")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptEur += result.TotalDept;
                }
            }

            foreach (var group in listBookedFlights)
            {
                var bookedUsers = group.Select(flight => new BookedUserInfoForMaratuk
                {
                    Id = flight.Id,
                    Name = flight.Name,
                    Surname = flight.Surname,
                    PhoneNumber = flight.PhoneNumber,
                    BirthDay = flight.BirthDay,
                    Email = flight.Email,
                    Passport = flight.Passport,
                    PasportExpiryDate = flight.PasportExpiryDate,
                    GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                }).ToList();

                var firstFlightInGroup = group.First();
                // You can take any flight from the group to extract common properties
                var bookedFlightResponse = new BookedFlightResponseForMaratuk
                {
                    bookedUsers = bookedUsers,
                    Id = firstFlightInGroup.Id,
                    OrderNumber = firstFlightInGroup.OrderNumber,
                    DateOfOrder = firstFlightInGroup.DateOfOrder,
                    ToureTypeId = firstFlightInGroup.ToureTypeId,
                    HotelId = firstFlightInGroup.HotelId,
                    TicketNumber = firstFlightInGroup.TicketNumber,
                    OrderStatusId = firstFlightInGroup.OrderStatusId,
                    TotalPrice = firstFlightInGroup.TotalPrice,
                    Rate = firstFlightInGroup.Rate,
                    AgentId = firstFlightInGroup.AgentId,//add agentName
                    AgentStatusId = firstFlightInGroup.AgentStatusId,
                    AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                    TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                    PassengersCount = firstFlightInGroup.PassengersCount,
                    TourStartDate = firstFlightInGroup.TourStartDate,
                    TourEndDate = firstFlightInGroup.TourEndDate,
                    DeadLine = firstFlightInGroup.DeadLine,
                    Paid = firstFlightInGroup.Paid,
                    MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                    MaratukAgentStatusId = firstFlightInGroup.MaratukAgentStatusId,
                    MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                    CountryId = firstFlightInGroup.CountryId,
                    CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                    Dept = firstFlightInGroup.Dept,
                    StartFlightId = firstFlightInGroup.StartFlightId,
                    EndFlightId = firstFlightInGroup.EndFlightId,
                    Comments = firstFlightInGroup.Comment,
                };

                if (bookedFlightResponse.HotelId != null)
                {
                    var hotel = _hotelManager.GetHotelByIdAsync((int)bookedFlightResponse.HotelId).Result;

                    if (hotel.Name != null)
                    {
                        bookedFlightResponse.HotelName = hotel.Name;

                    }
                    else
                    {
                        bookedFlightResponse.HotelName = string.Empty;

                    }
                }

                bookedFlightResponses.Add(bookedFlightResponse);
            }


            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;
            responseFinal.TotalPages = totalPages;

            return responseFinal;
        }


        public async Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightByMaratukAgentIdAsync(int maratukAgent, string? searchText,int pageNumber, int pageSize, DateTime? startDate = null, DateTime? endDate = null)
        {
            BookedFlightResponseFinalForMaratukAgent responseFinal = new BookedFlightResponseFinalForMaratukAgent();

            var listBookedFlightsAll = await _bookedFlightRepository.GetBookedFlightByMaratukAgentIdAsync(maratukAgent);


            List<BookedFlight?> filtred = new List<BookedFlight?>();

            if (startDate == null && endDate == null && searchText != null)
            {
                filtred = listBookedFlightsAll.Where(x => x.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            if (searchText == null && endDate == null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date >= startDate.Value.Date).ToList();
            }
            if (searchText == null && endDate != null && startDate == null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date <= endDate.Value.Date).ToList();
            }
            if (searchText == null && endDate != null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date <= endDate.Value.Date && item.DateOfOrder.Date >= startDate.Value.Date).ToList();
            }
            if (searchText != null && endDate == null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(x => x.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 && x.DateOfOrder.Date >= startDate.Value.Date).ToList();
            }
            if (searchText != null && endDate != null && startDate == null)
            {
                filtred = listBookedFlightsAll.Where(x => x.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 && x.DateOfOrder.Date <= endDate.Value.Date).ToList();
            }

            if (searchText != null && endDate != null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date <= endDate.Value.Date && item.DateOfOrder.Date >= startDate.Value.Date && item.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            var groupedBookedFlights = listBookedFlightsAll.GroupBy(flight => flight.OrderNumber).ToList();


            int distinctOrderNumbersCount = groupedBookedFlights.Count;




            var listBookedFlights = groupedBookedFlights
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();



            int totalPages = (int)Math.Ceiling((double)distinctOrderNumbersCount / pageSize);

            ///todo  add last actual currency


            var bookedFlightResponses = new List<BookedFlightResponseForMaratuk>();
            double totalDeptUsd = 0;
            double totalDeptEur = 0;



            var groupedFlights = filtred
                                .GroupBy(flight => new { flight.OrderNumber, flight.Rate })
                                .Select(group => new
                                {
                                    Currency = group.Key.Rate,
                                    TotalDept = group.Select(flight => flight.Dept ?? 0).Distinct().Sum()
                                });


            foreach (var result in groupedFlights)
            {

                if (result.Currency == "USD")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptUsd += result.TotalDept;
                }

                if (result.Currency == "EUR")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptEur += result.TotalDept;
                }
            }

            foreach (var group in listBookedFlights)
            {
                var bookedUsers = group.Select(flight => new BookedUserInfoForMaratuk
                {
                    Id = flight.Id,
                    Name = flight.Name,
                    Surname = flight.Surname,
                    PhoneNumber = flight.PhoneNumber,
                    BirthDay = flight.BirthDay,
                    Email = flight.Email,
                    Passport = flight.Passport,
                    PasportExpiryDate = flight.PasportExpiryDate,
                    GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                }).ToList();

                var firstFlightInGroup = group.First();
                // You can take any flight from the group to extract common properties
                var bookedFlightResponse = new BookedFlightResponseForMaratuk
                {
                    bookedUsers = bookedUsers,
                    Id = firstFlightInGroup.Id,
                    OrderNumber = firstFlightInGroup.OrderNumber,
                    DateOfOrder = firstFlightInGroup.DateOfOrder,
                    ToureTypeId = firstFlightInGroup.ToureTypeId,
                    HotelId = firstFlightInGroup.HotelId,
                    TicketNumber = firstFlightInGroup.TicketNumber,
                    OrderStatusId = firstFlightInGroup.OrderStatusId,
                    TotalPrice = firstFlightInGroup.TotalPrice,
                    Rate = firstFlightInGroup.Rate,
                    AgentId = firstFlightInGroup.AgentId,//add agentName
                    AgentStatusId = firstFlightInGroup.AgentStatusId,
                    AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                    TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                    PassengersCount = firstFlightInGroup.PassengersCount,
                    TourStartDate = firstFlightInGroup.TourStartDate,
                    TourEndDate = firstFlightInGroup.TourEndDate,
                    DeadLine = firstFlightInGroup.DeadLine,
                    Paid = firstFlightInGroup.Paid,
                    MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                    MaratukAgentStatusId = firstFlightInGroup.MaratukAgentStatusId,
                    MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                    CountryId = firstFlightInGroup.CountryId,
                    CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                    Dept = firstFlightInGroup.Dept,
                    StartFlightId = firstFlightInGroup.StartFlightId,
                    EndFlightId = firstFlightInGroup.EndFlightId,
                    Comments = firstFlightInGroup.Comment
                };

                if (bookedFlightResponse.HotelId != null)
                {
                    var hotel = _hotelManager.GetHotelByIdAsync((int)bookedFlightResponse.HotelId).Result;

                    if (hotel.Name != null)
                    {
                        bookedFlightResponse.HotelName = hotel.Name;

                    }
                    else
                    {
                        bookedFlightResponse.HotelName = string.Empty;

                    }
                }

                bookedFlightResponses.Add(bookedFlightResponse);
            }


            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;
            responseFinal.TotalPages = totalPages;

            return responseFinal;
        }

        public async Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn)
        {
            BookedFlightResponseFinal responseFinal = new BookedFlightResponseFinal();


            var response = await _userRepository.GetAgencyUsersAsync(Itn);

            var listBookedFlights = await _bookedFlightRepository.GetAllBookedFlightAsync(response);

            double totalDeptUsd = 0;
            double totalDeptEur = 0;

            var groupedBookedFlights = listBookedFlights.GroupBy(flight => flight.OrderNumber).ToList();


            var groupedFlights = listBookedFlights
     .GroupBy(flight => new { flight.OrderNumber, flight.Rate })
     .Select(group => new
     {
         Currency = group.Key.Rate,
         TotalDept = group.Select(flight => flight.Dept ?? 0).Distinct().Sum()
     });


            foreach (var result in groupedFlights)
            {

                if (result.Currency == "USD")
                {
                    totalDeptUsd += result.TotalDept;
                }

                if (result.Currency == "EUR")
                {
                    totalDeptEur += result.TotalDept;
                }
            }


            var bookedFlightResponses = new List<BookedFlightResponse>();

            foreach (var group in groupedBookedFlights)
            {
                var bookedUsers = group.Select(flight => new BookedUserInfo
                {
                    Name = flight.Name,
                    Surname = flight.Surname,
                    PhoneNumber = flight.PhoneNumber,
                    BirthDay = flight.BirthDay,
                    Email = flight.Email,
                    Passport = flight.Passport,
                    PasportExpiryDate = flight.PasportExpiryDate,
                    GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                    // Map other properties as needed
                }).ToList();

                var firstFlightInGroup = group.First(); // You can take any flight from the group to extract common properties
                var bookedFlightResponse = new BookedFlightResponse
                {
                    bookedUsers = bookedUsers,
                    Id = firstFlightInGroup.Id,
                    OrderNumber = firstFlightInGroup.OrderNumber,
                    DateOfOrder = firstFlightInGroup.DateOfOrder,
                    ToureTypeId = firstFlightInGroup.ToureTypeId,
                    HotelId = firstFlightInGroup.HotelId,
                    TicketNumber = firstFlightInGroup.TicketNumber,
                    OrderStatusId = firstFlightInGroup.OrderStatusId,
                    TotalPrice = firstFlightInGroup.TotalPrice,
                    Rate = firstFlightInGroup.Rate,
                    AgentId = firstFlightInGroup.AgentId,//add agentName
                    AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                    TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                    PassengersCount = firstFlightInGroup.PassengersCount,
                    TourStartDate = firstFlightInGroup.TourStartDate,
                    TourEndDate = firstFlightInGroup.TourEndDate,
                    DeadLine = firstFlightInGroup.DeadLine,
                    Paid = firstFlightInGroup.Paid,
                    MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                    MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                    CountryId = firstFlightInGroup.CountryId,
                    CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                    Dept = firstFlightInGroup.Dept,
                    StartFlightId = firstFlightInGroup.StartFlightId,
                    EndFlightId = firstFlightInGroup.EndFlightId,
                    Comments = firstFlightInGroup.Comment
                };

                bookedFlightResponses.Add(bookedFlightResponse);
            }

            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;

            return responseFinal;

        }

        public async Task<BookedFlight> UpdateBookedUserInfoAsync(BookedUserInfoForMaratukRequest bookedUserInfoForMaratuk)
        {
            var booked = await _bookedFlightRepository.GetBookedFlightByIdAsync(bookedUserInfoForMaratuk.Id);

            booked.Name = String.IsNullOrWhiteSpace(bookedUserInfoForMaratuk.Name) ? booked.Name : bookedUserInfoForMaratuk.Name;
            booked.Surname = String.IsNullOrWhiteSpace(bookedUserInfoForMaratuk.Surname) ? booked.Surname : bookedUserInfoForMaratuk.Surname;
            booked.PhoneNumber = String.IsNullOrWhiteSpace(bookedUserInfoForMaratuk.PhoneNumber) ? booked.PhoneNumber : bookedUserInfoForMaratuk.PhoneNumber;
            booked.Email = String.IsNullOrWhiteSpace(bookedUserInfoForMaratuk.Email) ? booked.Email : bookedUserInfoForMaratuk.Email;
            booked.Passport = String.IsNullOrWhiteSpace(bookedUserInfoForMaratuk.Passport) ? booked.Passport : bookedUserInfoForMaratuk.Passport;
            booked.PasportExpiryDate = bookedUserInfoForMaratuk.PasportExpiryDate;
            booked.BirthDay = bookedUserInfoForMaratuk.BirthDay;
            booked.GenderId = bookedUserInfoForMaratuk.GenderId;

            return await _mainRepository.UpdateAsync(booked);

        }

        public async Task<bool> UpdateBookedStatusAsync(string orderNumber, int status,string comment)
        {
            try
            {
                var booked = await _bookedFlightRepository.GetBookedFlightByOrderNumberAsync(orderNumber);

                foreach (var book in booked)
                {
                    book.OrderStatusId = status;
                    book.Comment = string.IsNullOrWhiteSpace(comment) ? string.Empty : comment;
                    await _mainRepository.UpdateAsync(book);
                }

      /*          if(booked != null)
                {
                    var maratukAcc = await _userRepository.GetUserAccAsync();

                    if(status == 2)
                    {
                        if (maratukAcc != null)
                        {
                            foreach (var acc in maratukAcc)
                            {
                                string email = acc.Email;

                                string date = DateTime.Now.ToString();

                                string textBody = $@"
                                    Agent: {orderNumber}
                                    Status: Manager Approved
                                    Date of sale: {date}";

                                MailService.SendEmail(email, $"New incaming Request {orderNumber}", textBody);

                            }

                        }
                    }
                    

                }*/

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightForAccAsync(int pageNumber, int pageSize)
        {
            BookedFlightResponseFinalForMaratukAgent responseFinal = new BookedFlightResponseFinalForMaratukAgent();

            var listBookedFlightsAll = await _bookedFlightRepository.GetBookedFlightByMaratukAgentForAccAsync();

            var groupedBookedFlights = listBookedFlightsAll.GroupBy(flight => flight.OrderNumber).ToList();

            int distinctOrderNumbersCount = groupedBookedFlights.Count;




            var listBookedFlights = groupedBookedFlights
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();



            int totalPages = (int)Math.Ceiling((double)distinctOrderNumbersCount / pageSize);

            var bookedFlightResponses = new List<BookedFlightResponseForMaratuk>();
            double totalDeptUsd = 0;
            double totalDeptEur = 0;



            var groupedFlights = listBookedFlightsAll
                                .GroupBy(flight => new { flight.OrderNumber, flight.Rate })
                                .Select(group => new
                                {
                                    Currency = group.Key.Rate,
                                    TotalDept = group.Select(flight => flight.Dept ?? 0).Distinct().Sum()
                                });


            foreach (var result in groupedFlights)
            {

                if (result.Currency == "USD")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptUsd += result.TotalDept;
                }

                if (result.Currency == "EUR")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptEur += result.TotalDept;
                }
            }

            foreach (var group in listBookedFlights)
            {
                var bookedHotels = _bookedHotelRepository.GetAllBookedHotelsAsync(group.Key);

                if (bookedHotels.Result == null)
                {
                    var bookedUsers = group.Select(flight => new BookedUserInfoForMaratuk
                    {
                        Id = flight.Id,
                        Name = flight.Name,
                        Surname = flight.Surname,
                        PhoneNumber = flight.PhoneNumber,
                        BirthDay = flight.BirthDay,
                        Email = flight.Email,
                        Passport = flight.Passport,
                        PasportExpiryDate = flight.PasportExpiryDate,
                        GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                    }).ToList();

                    var firstFlightInGroup = group.First();

                    var bookedFlightResponse = new BookedFlightResponseForMaratuk
                    {
                        bookedUsers = bookedUsers,
                        Id = firstFlightInGroup.Id,
                        OrderNumber = firstFlightInGroup.OrderNumber,
                        DateOfOrder = firstFlightInGroup.DateOfOrder,
                        ToureTypeId = firstFlightInGroup.ToureTypeId,
                        HotelId = firstFlightInGroup.HotelId,
                        TicketNumber = firstFlightInGroup.TicketNumber,
                        OrderStatusId = firstFlightInGroup.OrderStatusId,
                        TotalPrice = firstFlightInGroup.TotalPrice,
                        Rate = firstFlightInGroup.Rate,
                        AgentId = firstFlightInGroup.AgentId,//add agentName
                        AgentStatusId = firstFlightInGroup.AgentStatusId,
                        AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                        TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                        PassengersCount = firstFlightInGroup.PassengersCount,
                        TourStartDate = firstFlightInGroup.TourStartDate,
                        TourEndDate = firstFlightInGroup.TourEndDate,
                        DeadLine = firstFlightInGroup.DeadLine,
                        Paid = firstFlightInGroup.Paid,
                        MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                        MaratukAgentStatusId = firstFlightInGroup.MaratukAgentStatusId,
                        MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                        CountryId = firstFlightInGroup.CountryId,
                        CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                        Dept = firstFlightInGroup.Dept,
                        StartFlightId = firstFlightInGroup.StartFlightId,
                        EndFlightId = firstFlightInGroup.EndFlightId,
                        Comments = firstFlightInGroup.Comment
                    };

                    bookedFlightResponses.Add(bookedFlightResponse);
                }
                else
                {
                    if (bookedHotels.Result.OrderStatusId == 2 || bookedHotels.Result.OrderStatusId == 4)
                    {
                        var bookedUsers = group.Select(flight => new BookedUserInfoForMaratuk
                        {
                            Id = flight.Id,
                            Name = flight.Name,
                            Surname = flight.Surname,
                            PhoneNumber = flight.PhoneNumber,
                            BirthDay = flight.BirthDay,
                            Email = flight.Email,
                            Passport = flight.Passport,
                            PasportExpiryDate = flight.PasportExpiryDate,
                            GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                        }).ToList();

                        var firstFlightInGroup = group.First();

                        var bookedFlightResponse = new BookedFlightResponseForMaratuk
                        {
                            bookedUsers = bookedUsers,
                            Id = firstFlightInGroup.Id,
                            OrderNumber = firstFlightInGroup.OrderNumber,
                            DateOfOrder = firstFlightInGroup.DateOfOrder,
                            ToureTypeId = firstFlightInGroup.ToureTypeId,
                            HotelId = firstFlightInGroup.HotelId,
                            TicketNumber = firstFlightInGroup.TicketNumber,
                            OrderStatusId = firstFlightInGroup.OrderStatusId,
                            TotalPrice = firstFlightInGroup.TotalPrice,
                            Rate = firstFlightInGroup.Rate,
                            AgentId = firstFlightInGroup.AgentId,//add agentName
                            AgentStatusId = firstFlightInGroup.AgentStatusId,
                            AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                            TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                            PassengersCount = firstFlightInGroup.PassengersCount,
                            TourStartDate = firstFlightInGroup.TourStartDate,
                            TourEndDate = firstFlightInGroup.TourEndDate,
                            DeadLine = firstFlightInGroup.DeadLine,
                            Paid = firstFlightInGroup.Paid,
                            MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                            MaratukAgentStatusId = firstFlightInGroup.MaratukAgentStatusId,
                            MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                            CountryId = firstFlightInGroup.CountryId,
                            CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                            Dept = firstFlightInGroup.Dept,
                            StartFlightId = firstFlightInGroup.StartFlightId,
                            EndFlightId = firstFlightInGroup.EndFlightId,
                            Comments = firstFlightInGroup.Comment
                        };

                        if (bookedFlightResponse.HotelId != null)
                        {
                            var hotel = _hotelManager.GetHotelByIdAsync((int)bookedFlightResponse.HotelId).Result;

                            if (hotel.Name != null)
                            {
                                bookedFlightResponse.HotelName = hotel.Name;

                            }
                            else
                            {
                                bookedFlightResponse.HotelName = string.Empty;

                            }
                        }

                        bookedFlightResponses.Add(bookedFlightResponse);
                    }

                }

            }


            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;
            responseFinal.TotalPages = totalPages;

            return responseFinal;
        }

        public async Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightForAccAsync(int pageNumber, int pageSize, string? searchText, DateTime? startDate, DateTime? endDate)
        {
            BookedFlightResponseFinalForMaratukAgent responseFinal = new BookedFlightResponseFinalForMaratukAgent();

            var listBookedFlightsAll = await _bookedFlightRepository.GetBookedFlightByMaratukAgentForAccAsync();


            List<BookedFlight?> filtred = new List<BookedFlight?>();

            if (startDate == null && endDate == null && searchText != null)
            {
                filtred = listBookedFlightsAll.Where(x => x.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            if (searchText == null && endDate == null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date >= startDate.Value.Date).ToList();
            }
            if (searchText == null && endDate != null && startDate == null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date <= endDate.Value.Date).ToList();
            }
            if (searchText == null && endDate != null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date <= endDate.Value.Date && item.DateOfOrder.Date >= startDate.Value.Date).ToList();
            }
            if (searchText != null && endDate == null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(x => x.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 && x.DateOfOrder.Date >= startDate.Value.Date).ToList();
            }
            if (searchText != null && endDate != null && startDate == null)
            {
                filtred = listBookedFlightsAll.Where(x => x.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 && x.DateOfOrder.Date <= endDate.Value.Date).ToList();
            }

            if (searchText != null && endDate != null && startDate != null)
            {
                filtred = listBookedFlightsAll.Where(item => item.DateOfOrder.Date <= endDate.Value.Date && item.DateOfOrder.Date >= startDate.Value.Date && item.OrderNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }


            var groupedBookedFlights = filtred.GroupBy(flight => flight.OrderNumber).ToList();

            int distinctOrderNumbersCount = groupedBookedFlights.Count;

            var listBookedFlights = groupedBookedFlights
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();



            int totalPages = (int)Math.Ceiling((double)distinctOrderNumbersCount / pageSize);

            var bookedFlightResponses = new List<BookedFlightResponseForMaratuk>();
            double totalDeptUsd = 0;
            double totalDeptEur = 0;



            var groupedFlights = filtred
                                .GroupBy(flight => new { flight.OrderNumber, flight.Rate })
                                .Select(group => new
                                {
                                    Currency = group.Key.Rate,
                                    TotalDept = group.Select(flight => flight.Dept ?? 0).Distinct().Sum()
                                });


            foreach (var result in groupedFlights)
            {

                if (result.Currency == "USD")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptUsd += result.TotalDept;
                }

                if (result.Currency == "EUR")
                {
                    // Assuming a specific USD rate, adjust the calculation as needed
                    totalDeptEur += result.TotalDept;
                }
            }

            foreach (var group in listBookedFlights)
            {
                var bookedHotels = _bookedHotelRepository.GetAllBookedHotelsAsync(group.Key);

                if (bookedHotels.Result == null)
                {
                    var bookedUsers = group.Select(flight => new BookedUserInfoForMaratuk
                    {
                        Id = flight.Id,
                        Name = flight.Name,
                        Surname = flight.Surname,
                        PhoneNumber = flight.PhoneNumber,
                        BirthDay = flight.BirthDay,
                        Email = flight.Email,
                        Passport = flight.Passport,
                        PasportExpiryDate = flight.PasportExpiryDate,
                        GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                    }).ToList();

                    var firstFlightInGroup = group.First();

                    var bookedFlightResponse = new BookedFlightResponseForMaratuk
                    {
                        bookedUsers = bookedUsers,
                        Id = firstFlightInGroup.Id,
                        OrderNumber = firstFlightInGroup.OrderNumber,
                        DateOfOrder = firstFlightInGroup.DateOfOrder,
                        ToureTypeId = firstFlightInGroup.ToureTypeId,
                        HotelId = firstFlightInGroup.HotelId,
                        TicketNumber = firstFlightInGroup.TicketNumber,
                        OrderStatusId = firstFlightInGroup.OrderStatusId,
                        TotalPrice = firstFlightInGroup.TotalPrice,
                        Rate = firstFlightInGroup.Rate,
                        AgentId = firstFlightInGroup.AgentId,//add agentName
                        AgentStatusId = firstFlightInGroup.AgentStatusId,
                        AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                        TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                        PassengersCount = firstFlightInGroup.PassengersCount,
                        TourStartDate = firstFlightInGroup.TourStartDate,
                        TourEndDate = firstFlightInGroup.TourEndDate,
                        DeadLine = firstFlightInGroup.DeadLine,
                        Paid = firstFlightInGroup.Paid,
                        MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                        MaratukAgentStatusId = firstFlightInGroup.MaratukAgentStatusId,
                        MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                        CountryId = firstFlightInGroup.CountryId,
                        CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                        Dept = firstFlightInGroup.Dept,
                        StartFlightId = firstFlightInGroup.StartFlightId,
                        EndFlightId = firstFlightInGroup.EndFlightId,
                        Comments = firstFlightInGroup.Comment
                    };

                    bookedFlightResponses.Add(bookedFlightResponse);
                }
                else
                {
                    if (bookedHotels.Result.OrderStatusId == 2 || bookedHotels.Result.OrderStatusId == 4)
                    {
                        var bookedUsers = group.Select(flight => new BookedUserInfoForMaratuk
                        {
                            Id = flight.Id,
                            Name = flight.Name,
                            Surname = flight.Surname,
                            PhoneNumber = flight.PhoneNumber,
                            BirthDay = flight.BirthDay,
                            Email = flight.Email,
                            Passport = flight.Passport,
                            PasportExpiryDate = flight.PasportExpiryDate,
                            GenderName = (flight.GenderId == 1) ? "Male" : "Female"
                        }).ToList();

                        var firstFlightInGroup = group.First();

                        var bookedFlightResponse = new BookedFlightResponseForMaratuk
                        {
                            bookedUsers = bookedUsers,
                            Id = firstFlightInGroup.Id,
                            OrderNumber = firstFlightInGroup.OrderNumber,
                            DateOfOrder = firstFlightInGroup.DateOfOrder,
                            ToureTypeId = firstFlightInGroup.ToureTypeId,
                            HotelId = firstFlightInGroup.HotelId,
                            TicketNumber = firstFlightInGroup.TicketNumber,
                            OrderStatusId = firstFlightInGroup.OrderStatusId,
                            TotalPrice = firstFlightInGroup.TotalPrice,
                            Rate = firstFlightInGroup.Rate,
                            AgentId = firstFlightInGroup.AgentId,//add agentName
                            AgentStatusId = firstFlightInGroup.AgentStatusId,
                            AgentName = _userRepository.GetAgencyUsersByIdAsync(firstFlightInGroup.AgentId).Result.FullName,
                            TotalPriceAmd = firstFlightInGroup.TotalPriceAmd,
                            PassengersCount = firstFlightInGroup.PassengersCount,
                            TourStartDate = firstFlightInGroup.TourStartDate,
                            TourEndDate = firstFlightInGroup.TourEndDate,
                            DeadLine = firstFlightInGroup.DeadLine,
                            Paid = firstFlightInGroup.Paid,
                            MaratukAgentId = firstFlightInGroup.MaratukAgentId,
                            MaratukAgentStatusId = firstFlightInGroup.MaratukAgentStatusId,
                            MaratukAgentName = _userRepository.GetUserByIdAsync(firstFlightInGroup.MaratukAgentId).Result.UserName,
                            CountryId = firstFlightInGroup.CountryId,
                            CountryName = _countryManager.GetCountryNameByIdAsync(firstFlightInGroup.CountryId).Result.NameENG,
                            Dept = firstFlightInGroup.Dept,
                            StartFlightId = firstFlightInGroup.StartFlightId,
                            EndFlightId = firstFlightInGroup.EndFlightId,
                            Comments = firstFlightInGroup.Comment
                        };

                        if (bookedFlightResponse.HotelId != null)
                        {
                            var hotel = _hotelManager.GetHotelByIdAsync((int)bookedFlightResponse.HotelId).Result;

                            if (hotel.Name != null)
                            {
                                bookedFlightResponse.HotelName = hotel.Name;

                            }
                            else
                            {
                                bookedFlightResponse.HotelName = string.Empty;

                            }
                        }

                        bookedFlightResponses.Add(bookedFlightResponse);
                    }

                }

            }


            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;
            responseFinal.TotalPages = totalPages;

            return responseFinal;
        }

    }
}
