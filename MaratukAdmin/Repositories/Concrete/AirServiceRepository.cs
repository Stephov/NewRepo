using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class AirServiceRepository : MainRepository<AirService>, IAirServiceRepository
    {
        public AirServiceRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
