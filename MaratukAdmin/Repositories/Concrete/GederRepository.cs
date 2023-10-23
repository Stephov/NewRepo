using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class GenderRepository : MainRepository<Gender>, IGenderRepository
    {
        public GenderRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
