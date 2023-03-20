using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class AircraftRepository : MainRepository<Aircraft>, IAircraftRepository
    {
        public AircraftRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
