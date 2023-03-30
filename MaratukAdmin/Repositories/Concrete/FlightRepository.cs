using MaratukAdmin.Entities;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class FlightRepository : IFlightRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public FlightRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
        {
            return await _dbContext.Flight
                //.Include(f => f.Schedules)
                .ToListAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int id)
        {
            return await _dbContext.Flight
                //.Include(f => f.Schedules)
                .SingleOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Flight> CreateFlightAsync(Flight flight)
        {
            try
            {
                await _dbContext.Flight.AddAsync(flight);
                await _dbContext.SaveChangesAsync();

            }catch(Exception ex)
            {

                string a = ex.Message;
            }
            


            return flight;
        }

        public async Task UpdateFlightAsync(Flight flight)
        {
            _dbContext.Flight.Update(flight);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFlightAsync(int id)
        {
            var flight = await GetFlightByIdAsync(id);
            if (flight == null)
            {
                throw new ArgumentException($"Flight with id {id} does not exist.");
            }

            _dbContext.Flight.Remove(flight);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByFlightIdAsync(int flightId)
        {
            return await _dbContext.Schedule
                .Where(s => s.FlightId == flightId)
                .ToListAsync();
        }

        public async Task CreateScheduleAsync(Schedule schedule)
        {
            await _dbContext.Schedule.AddAsync(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            _dbContext.Schedule.Update(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _dbContext.Schedule.FindAsync(id);
            if (schedule == null)
            {
                throw new ArgumentException($"Schedule with id {id} does not exist.");
            }

            _dbContext.Schedule.Remove(schedule);
            await _dbContext.SaveChangesAsync();
        }
    }
}
