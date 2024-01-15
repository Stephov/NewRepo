using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Tnef;
using System;
using System.Collections.Generic;

namespace MaratukAdmin.Managers.Concrete
{
    public class BookedFlightManager : IBookedFlightManager
    {

        //private readonly IMainRepository<BookedFlight> _mainRepository;
        private readonly IBookedFlightRepository _bookedFlightRepository;
        private readonly ICountryManager _countryManager;
        private readonly IFlightRepository _flightRepository;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public BookedFlightManager(IMapper mapper, IBookedFlightRepository bookedFlightRepository,
            ICountryManager countryManager,
            IUserRepository userRepository,
            ICurrencyRatesRepository currencyRatesRepository,
            IFlightRepository flightRepository)
        {

            _bookedFlightRepository = bookedFlightRepository;
            _countryManager = countryManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _currencyRatesRepository = currencyRatesRepository;
            _flightRepository = flightRepository;
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
            double totalDeptUsd = listBookedFlights
    .Where(bf => bf.Rate == "USD")
    .Sum(bf => bf.Dept ?? 0);


            double totalDeptEur = listBookedFlights
   .Where(bf => bf.Rate == "EUR")
   .Sum(bf => bf.Dept ?? 0);




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
                };

                bookedFlightResponses.Add(bookedFlightResponse);
            }


            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;

            return responseFinal;
        }

        public async Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn)
        {
            BookedFlightResponseFinal responseFinal = new BookedFlightResponseFinal();


            var response = await _userRepository.GetAgencyUsersAsync(Itn);

            var listBookedFlights = await _bookedFlightRepository.GetAllBookedFlightAsync(response);

            double totalDeptUsd = listBookedFlights
   .Where(bf => bf.Rate == "USD")
   .Sum(bf => bf.Dept ?? 0);


            double totalDeptEur = listBookedFlights
   .Where(bf => bf.Rate == "EUR")
   .Sum(bf => bf.Dept ?? 0);

            var groupedBookedFlights = listBookedFlights.GroupBy(flight => flight.OrderNumber).ToList();

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
                };

                bookedFlightResponses.Add(bookedFlightResponse);
            }

            responseFinal.bookedFlightResponses = bookedFlightResponses;
            responseFinal.DeptUSD = (int)totalDeptUsd * -1;
            responseFinal.DeptEUR = (int)totalDeptEur * -1;

            return responseFinal;

        }

    }
}
