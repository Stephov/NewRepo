using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract.Sansejour;

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
    }
}
