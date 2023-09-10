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
