using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class FunctionRepository : IFunctionRepository
    {
        protected readonly MaratukDbContext _context;

        public FunctionRepository(MaratukDbContext context)
        {
            _context = context;
        }

        public async Task<List<FlightReturnDate>> GetFlightReturnDateAsync(int PriceBlockId, int DepartureCountryId, int DepartureCityId, int DestinationCountryId, int DestinationCityId)
        {
            try
            {
                var results = await _context.FlightReturnDate
           .FromSqlRaw("EXEC GetFlightReturnDate @p0, @p1, @p2, @p3, @p4", PriceBlockId, DepartureCountryId, DepartureCityId, DestinationCountryId, DestinationCityId)
           .ToListAsync();
                return results;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return null;
        }

        public async Task<List<FlightReturnDateForManual>> GetFlightReturnDateForManualAsync(DateTime Date, int FlightId)
        {
            try
            {
                var results = await _context.FlightReturnDateForManual
           .FromSqlRaw("EXEC GetFlightReturnDateForManual @p0, @p1", Date, FlightId)
           .ToListAsync();
                return results;

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return null;
        }

        public async Task<List<FlightInfoFunction>> GetFligthInfoFunctionAsync(int TripTypeId)
        {
            try
            {
                var results = await _context.FlightInfoResults
           .FromSqlRaw("EXEC GetFlightInfoByTripType @p0", TripTypeId)
           .ToListAsync();
                return results;

            }
            catch (Exception ex) { 
            string message = ex.Message;
            }

            return null;
        }
    }
}
