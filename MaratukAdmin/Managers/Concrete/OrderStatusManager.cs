using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;

namespace MaratukAdmin.Managers.Concrete
{
    public class OrderStatusManager : IOrderStatusManager
    {

        private readonly IOrderStatusRepository _orderStatusRepository;


        public OrderStatusManager(IOrderStatusRepository orderStatusRepository)
        {
            _orderStatusRepository = orderStatusRepository;
        }

        public async Task<List<OrderStatus>> GetOrderStatusAsync()
        {
            var result = await _orderStatusRepository.GetAllAsync();
            return result;
        }
    }
}
