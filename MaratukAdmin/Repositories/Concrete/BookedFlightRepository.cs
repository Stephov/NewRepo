using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

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
                                   .Where(c => c.MaratukAgentId == maratukAgent)
                                   .ToListAsync();

            return result;
        }


    }
}
