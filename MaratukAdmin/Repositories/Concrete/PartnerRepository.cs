using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class PartnerRepository : MainRepository<Partner>, IPartnerRepository
    {
        public PartnerRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
