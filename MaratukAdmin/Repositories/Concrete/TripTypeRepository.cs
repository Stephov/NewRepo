using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class TripTypeRepository : MainRepository<TripType>, ITripTypeRepository
    {
        public TripTypeRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
