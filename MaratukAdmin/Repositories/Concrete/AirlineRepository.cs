using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class AirlineRepository : MainRepository<Airline>, IAirlineRepository
    {
        public AirlineRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
