using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Repositories.Concrete
{
    public class CountryRepository : MainRepository<Country>, ICountryRepository
    {
        public CountryRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
