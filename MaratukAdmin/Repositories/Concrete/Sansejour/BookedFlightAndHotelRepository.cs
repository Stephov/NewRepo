using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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

        public async Task<BookPayment> GetBookPaymentAsync(int? id, string? orderNumber, string? paymentNumber)
        {
            return await _dbContext.BookPayments.Where(c => c.Id == (id ?? c.Id)
                                                        && c.OrderNumber == (orderNumber ?? c.OrderNumber)
                                                        && c.PaymentNumber == (paymentNumber ?? c.PaymentNumber)
                                                        ).FirstOrDefaultAsync();
        }


        public async Task<List<BookPayment>> GetBookPaymentsByOrderNumberAndPaymentStatusAsync(string orderNumber, int? paymentStatus = null)
        {
            //return await _dbContext.BookPayments.Where(c => c.OrderNumber == orderNumber
            //                                            && c.PaymentStatus != (int)Enums.enumBookPaymentStatuses.Declined
            //                                            && c.PaymentStatus != (int)Enums.enumBookPaymentStatuses.Cancelled
            //                                            && c.PaymentStatus == (paymentStatus ?? c.PaymentStatus)
            //                                            ).ToListAsync();

            IQueryable<BookPayment> query = _dbContext.BookPayments.Where(c => c.OrderNumber == orderNumber);

            if (paymentStatus == null)
            {
                query = query.Where(c => c.PaymentStatus != (int)Enums.enumBookPaymentStatuses.Declined
                                      && c.PaymentStatus != (int)Enums.enumBookPaymentStatuses.Cancelled);
            }
            else
            {
                query = query.Where(c => c.PaymentStatus == paymentStatus);
            }

            var retValue = await query.ToListAsync();
            
            //return await query.ToListAsync();
            return retValue;
        }

        public async Task UpdateBookPaymentAsync(BookPayment bookPayment)
        {
            _dbContext.BookPayments.Update(bookPayment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
