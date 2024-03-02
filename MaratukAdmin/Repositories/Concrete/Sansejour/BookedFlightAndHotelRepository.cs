using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{
    public class BookedFlightAndHotelRepository : IBookedFlightAndHotelRepository
    {

        protected readonly MaratukDbContext _dbContext;

        public BookedFlightAndHotelRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BookedInfoFlightPartResponse>> GetBookedInfoFlighPartAsync(BookedInfoFlightPartRequest request)
        {

        try
            {
                var result = await _dbContext.BookedInfoFlightPartResponse.FromSqlRaw("EXEC dbo.GetBookedInfoFlightPart " +
                                                                    "@CountryId, @CityId , @StartDate, @EndDate," +
                                                                    "@StartFlightId",
                                                                    //"@EndFlightId",
                                                                    new SqlParameter("CountryId", request.CountryId),
                                                                    new SqlParameter("CityId", request.CityId),
                                                                    new SqlParameter("StartDate", request.StartDate),
                                                                    new SqlParameter("EndDate", request.EndDate),
                                                                    new SqlParameter("StartFlightId", request.StartFlightId)
                                                                    //new SqlParameter("EndFlightId", request.EndFlightId)
                                                                    ).ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PayForBookedFlightAndHotelAsync(PayForBookedFlightAndHotelRequest payForBookedFlightAndHotel)
        {
            return "OK";
        }
    }
}
