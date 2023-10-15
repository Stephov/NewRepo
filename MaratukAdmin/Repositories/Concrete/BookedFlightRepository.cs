﻿using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaratukAdmin.Repositories.Concrete
{
    public class BookedFlightRepository :  IBookedFlightRepository
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

        public async Task<List<BookedFlight>> GetAllBookedFlightAsync()
        {
            var result = await _dbContext.BookedFlights
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

    }
}
