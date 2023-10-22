﻿using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Utils;
using System;

namespace MaratukAdmin.Managers.Concrete
{
    public class BookedFlightManager : IBookedFlightManager
    {

        //private readonly IMainRepository<BookedFlight> _mainRepository;
        private readonly IBookedFlightRepository _bookedFlightRepository;
        private readonly IMapper _mapper;


        public BookedFlightManager( IMapper mapper, IBookedFlightRepository bookedFlightRepository)
        {
           
            _bookedFlightRepository = bookedFlightRepository;
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




                    await _bookedFlightRepository.CreateBookedFlightAsync(booked);

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int id)
        {
            return await _bookedFlightRepository.GetBookedFlightByAgentIdAsync(id);
        }

        public async Task<List<BookedFlight>> GetBookedFlightAsync()
        {
            var result = await _bookedFlightRepository.GetAllBookedFlightAsync();
            return result;
        }


    }
}
