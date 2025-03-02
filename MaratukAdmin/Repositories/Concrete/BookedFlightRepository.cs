﻿using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MaratukAdmin.Repositories.Concrete
{
    public class BookedFlightRepository : IBookedFlightRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public BookedFlightRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookedFlight> CreateBookedFlightAsync(BookedFlight bookedFlight)
        {
            await _dbContext.BookedFlights.AddAsync(bookedFlight);
            await _dbContext.SaveChangesAsync();

            return bookedFlight;
        }

        public async Task<List<BookedFlight>> GetAllBookedFlightAsync(List<AgencyUser> agencyUsers)
        {
            var agentIds = agencyUsers.Select(au => au.Id).ToList();

            var result = await _dbContext.BookedFlights
                                           .Where(bf => agentIds.Contains(bf.AgentId))
                                   .ToListAsync();

            return result;
        }

        /* public BookedFlightRepository(MaratukDbContext context) : base(context)
         {
         }*/

        public async Task<List<BookedFlight>> GetBookedFlightByAgentIdAsync(int agentId)
        {
            var result = await _dbContext.BookedFlights
                                   .Where(c => c.AgentId == agentId)
                                   .ToListAsync();

            return result;
        }

        public async Task<BookedFlight> GetBookedFlightByIdAsync(int Id)
        {
            var result = await _dbContext.BookedFlights
                                   .Where(c => c.Id == Id)
                                   .FirstAsync();

            return result;
        }

        public async Task<List<BookedFlight>> GetBookedFlightByOrderNumberAsync(string orderNumber)
        {
            var result = await _dbContext.BookedFlights
                                   .Where(c => c.OrderNumber == orderNumber)
                                   .ToListAsync();

            return result;
        }

        public async Task<List<BookedFlight>> GetBookedFlightByMaratukAgentIdAsync(int maratukAgent)
        {

            var result = await _dbContext.BookedFlights
                .Where(c => c.MaratukFlightAgentId == maratukAgent)
                .ToListAsync();

            return result;
        }


        public async Task<List<BookedFlight>> GetBookedFlightForHotelManagerAsync(int maratukAgent)
        {

            var result = await _dbContext.BookedFlights
                .Where(c => c.HotelId != null && c.MaratukHotelAgentId == maratukAgent)
                .ToListAsync();

            return result;
        }



        public async Task<List<BookedFlight>> GetBookedFlightByMaratukAgentForAccAsync()
        {

            var result = await _dbContext.BookedFlights
                //.Where(c => c.BookStatusForMaratuk == 7 || c.BookStatusForMaratuk == 12 || c.BookStatusForMaratuk == 13)
                .Where(c => c.BookStatusForMaratuk >= 7 && c.BookStatusForMaratuk <= 13)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetBookedFlightCountAsync()
        {
            var counts = await _dbContext.BookedFlights
           .GroupBy(bf => bf.OrderNumber)
           .Select(group => new
           {
               OrderNumber = group.Key,
               Count = group.Count()
           })
           .ToListAsync();

            return counts.Count;
        }

        public async Task UpdateBookedFlightsAsync(List<BookedFlight> bookedFlights)
        {
            //foreach (var bookedFlight in bookedFlights)
            //{
            //    _dbContext.Entry(bookedFlight).State = EntityState.Modified;
            //}

            _dbContext.BookedFlights.UpdateRange(bookedFlights);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ReturnStatusResponse> SetTicketNumberToBookAsync(SetTicketNumberToBookRequest request)
        {
            ReturnStatusResponse retValue = new()
            {
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "OK"
            };

            try
            {
                foreach (var ticket in request.Tikets)
                {
                    var bookedFlight = await GetBookedFlightByIdAsync(ticket.BookId);
                    if (bookedFlight != null)
                    {
                        bookedFlight.TicketNumber = ticket.TicketNumber;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                retValue.StatusCode = StatusCodes.Status400BadRequest;
                retValue.StatusMessage = ex.Message;
            }
            return retValue;
        }

    }
}
