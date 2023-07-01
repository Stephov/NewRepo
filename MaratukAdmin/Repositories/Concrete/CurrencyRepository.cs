using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class CurrencyRepository : MainRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
