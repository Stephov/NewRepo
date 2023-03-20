using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Repositories.Concrete
{
    public class PricePackageRepository : MainRepository<PricePackage>, IPricePackageRepository
    {
        public PricePackageRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
