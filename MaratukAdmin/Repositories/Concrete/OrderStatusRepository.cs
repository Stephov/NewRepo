using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class OrderStatusRepository : MainRepository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
