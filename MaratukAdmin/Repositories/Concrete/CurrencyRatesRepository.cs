using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class CurrencyRatesRepository : MainRepository<CurrencyRates>, ICurrencyRatesRepository
    {
        public CurrencyRatesRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
