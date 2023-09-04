using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class PriceBlockStateRepository : MainRepository<PriceBlockState>, IPriceBlockStateRepository
    {
        public PriceBlockStateRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
