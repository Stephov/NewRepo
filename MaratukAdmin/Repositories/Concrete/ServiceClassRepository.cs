using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class ServiceClassRepository : MainRepository<ServiceClass>, IServiceClassRepository
    {
        public ServiceClassRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
