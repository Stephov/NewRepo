using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface ITransactionRepository
    {
        Task BeginTransAsync();
        Task CommitTransAsync();
        Task RollbackTransAsync();
    }
}
