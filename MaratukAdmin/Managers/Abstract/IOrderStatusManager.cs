using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IOrderStatusManager
    {
        Task<List<OrderStatus>> GetOrderStatusAsync();
    }
}
