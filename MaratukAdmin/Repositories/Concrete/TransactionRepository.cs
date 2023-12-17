using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MaratukAdmin.Repositories.Concrete
{
    public class TransactionRepository : ITransactionRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public TransactionRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task BeginTransAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _dbContext.Database.CreateExecutionStrategy ();
        }
    }
}
