using MaratukAdmin.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface ITransactionRepository
    {
        Task BeginTransAsync();
        Task CommitTransAsync();
        Task RollbackTransAsync();
        IExecutionStrategy CreateExecutionStrategy();
    }
}
