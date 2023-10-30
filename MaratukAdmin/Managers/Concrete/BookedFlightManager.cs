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
using System;

namespace MaratukAdmin.Managers.Concrete
{
    public class BookedFlightManager : IBookedFlightManager
    {

        //private readonly IMainRepository<BookedFlight> _mainRepository;
        private readonly IBookedFlightRepository _bookedFlightRepository;
        private readonly ICountryManager _countryManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public BookedFlightManager(IMapper mapper, IBookedFlightRepository bookedFlightRepository, 
            ICountryManager countryManager,
            IUserRepository userRepository)
        {

            _bookedFlightRepository = bookedFlightRepository;
            _countryManager = countryManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddBookedFlightAsync(List<AddBookedFlight> addBookedFlight)
        {
            try
            {
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
                    booked.TotalPriceAmd = bookedFlight.TotalPriceAmd;
                    booked.PassengersCount = bookedFlight.PassengersCount;
                    booked.TourStartDate = bookedFlight.TourStartDate;
                    booked.TourEndDate = bookedFlight.TourEndDate;
                    booked.MaratukAgentId = bookedFlight.MaratukAgentId;
                    booked.CountryId = bookedFlight.CountryId;
                    booked.StartFlightId = bookedFlight.StartFlightId;
                    booked.EndFlightId = bookedFlight.EndFlightId;
                    booked.PasportExpiryDate = bookedFlight.PasportExpiryDate;
                    booked.GenderId = bookedFlight.GenderId;




                    await _bookedFlightRepository.CreateBookedFlightAsync(booked);

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<BookedFlightResponse>> GetBookedFlightByAgentIdAsync(int id)
        {
            var listBookedFlights = await _bookedFlightRepository.GetBookedFlightByAgentIdAsync(id);

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

            


            return bookedFlightResponses;
        }

        public async Task<List<BookedFlightResponse>> GetBookedFlightAsync()
        {
            var listBookedFlights = await _bookedFlightRepository.GetAllBookedFlightAsync();

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
            return bookedFlightResponses;


        }

    }
}
