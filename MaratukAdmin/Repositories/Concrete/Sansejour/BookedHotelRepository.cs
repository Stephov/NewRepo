﻿using MailKit.Search;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{
    public class BookedHotelRepository : IBookedHotelRepository
    {

        protected readonly MaratukDbContext _dbContext;

        public BookedHotelRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookedHotel> CreateBookedHotelAsync(BookedHotel bookedHotel)
        {
            try
            {
                await _dbContext.BookedHotel.AddAsync(bookedHotel);
                //await _dbContext.BookedHotelGuest.AddRangeAsync(bookedHotelGuests);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return bookedHotel;
        }

        public async Task AddBookedHotelGuestsAsync(List<BookedHotelGuest> bookedHotelGuests)
        {
            try
            {
                await _dbContext.BookedHotelGuest.AddRangeAsync(bookedHotelGuests);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BookInvoiceData> AddBookedHotelInvoiceDataAsync(BookInvoiceData invoiceData)
        {
            try
            {
                await _dbContext.BookInvoiceData.AddAsync(invoiceData);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return invoiceData;
        }

        public async Task<BookInvoiceData>? GetBookInvoiceDataAsync(string orderNumber)
        {

            return await _dbContext.BookInvoiceData
                        .Where(o => o.OrderNumber == orderNumber)
                        .FirstOrDefaultAsync();
        }
        public async Task<List<BookedHotelResponse>> GetAllBookedHotelsAsync(List<AgencyUser> agencyUsers)
        {
            var agentIds = agencyUsers.Select(au => au.Id).ToList();

            //var result = await (from bh in _dbContext.BookedHotel
            //                    join g in _dbContext.BookedHotelGuest on bh.OrderNumber equals g.OrderNumber
            //                    where agentIds.Contains(bh.AgentId)
            //                    select new BookedHotelResponse
            //                    {
            //                        BookedHotel = bh,
            //                        BookedHotelGuests = _dbContext.BookedHotelGuest
            //                                              .Where(gh => gh.OrderNumber == bh.OrderNumber)
            //                                              .ToList()
            //                    }).ToListAsync();

            //return result;

            return new List<BookedHotelResponse>();
        }

        public async Task<BookedHotel> GetBookedHotelByOrderNumberAsync(string orderID)
        {
            //List<int> orderStatusIds = new List<int> { 2, 4, 5 };


            return await _dbContext.BookedHotel
                .Where(o => o.OrderNumber == orderID)
                .FirstOrDefaultAsync();
        }


        public async Task<BookedHotel> UpdateBookedHotelAsync(BookedHotel bookedHotel)
        {
            try
            {
                _dbContext.BookedHotel.Update(bookedHotel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return bookedHotel;
        }
    }
}
