using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class TarifRepository : MainRepository<Tarif>, ITarifRepository
    {
        public TarifRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
