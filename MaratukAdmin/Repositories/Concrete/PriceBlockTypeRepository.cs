using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class PriceBlockTypeRepository : MainRepository<PriceBlockType>, IPriceBlockTypeRepository
    {
        public PriceBlockTypeRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
