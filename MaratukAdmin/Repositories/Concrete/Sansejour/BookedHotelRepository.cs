﻿using MaratukAdmin.Dto.Response.Sansejour;
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

        public async Task<BookedHotel> CreateBookedHotelAsync(BookedHotel bookedHotel, List<BookedHotelGuest> bookedHotelGuests)
        {
            try
            {
                await _dbContext.BookedHotel.AddAsync(bookedHotel);
                await _dbContext.BookedHotelGuest.AddRangeAsync(bookedHotelGuests);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return bookedHotel;
        }

        public async Task<List<BookedHotelResponse>> GetAllBookedHotelsAsync(List<AgencyUser> agencyUsers)
        {
            var agentIds = agencyUsers.Select(au => au.Id).ToList();

            var result = await (from bh in _dbContext.BookedHotel
                                join g in _dbContext.BookedHotelGuest on bh.OrderNumber equals g.OrderNumber
                                where agentIds.Contains(bh.AgentId)
                                select new BookedHotelResponse
                                {
                                    BookedHotel = bh,
                                    BookedHotelGuests = _dbContext.BookedHotelGuest
                                                          .Where(gh => gh.OrderNumber == bh.OrderNumber)
                                                          .ToList()
                                }).ToListAsync();

            return result;
        }
    }
}
