using MaratukAdmin.Entities;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class PriceBlockRepository : IPriceBlockRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public PriceBlockRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PriceBlock>> GetAllPriceBlocksAsync()
        {
            return await _dbContext.PriceBlocks
                //.Include(f => f.PriceBlockServices)
                .ToListAsync();
        }

        public async Task<PriceBlock> GetPriceBlockByIdAsync(int id)
        {
            return await _dbContext.PriceBlocks
                //.Include(f => f.PriceBlockServices)
                .SingleOrDefaultAsync(f => f.Id == id);
        }

        public async Task<PriceBlock> CreatePriceBlockAsync(PriceBlock priceBlock)
        {
            try
            {
                await _dbContext.PriceBlocks.AddAsync(priceBlock);
                await _dbContext.SaveChangesAsync();

            }catch(Exception ex)
            {

                string a = ex.Message;
            }
            
            return priceBlock;
        }

        public async Task UpdatePriceBlockAsync(PriceBlock priceBlock)
        {
            _dbContext.PriceBlocks.Update(priceBlock);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePriceBlockAsync(int id)
        {
            var priceBlock = await GetPriceBlockByIdAsync(id);
            if (priceBlock == null)
            {
                throw new ArgumentException($"Price Block with id {id} does not exist.");
            }

            _dbContext.PriceBlocks.Remove(priceBlock);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<PriceBlockServices>> GetPriceBlockServicesByPriceBlockIdAsync(int priceBlockId)
        {
            return await _dbContext.PriceBlockServices
                .Where(s => s.PriceBlockId == priceBlockId)
                .ToListAsync();
        }

        public async Task CreatePriceBlockServicesAsync(PriceBlockServices priceBlockServices)
        {
            await _dbContext.PriceBlockServices.AddAsync(priceBlockServices);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePriceBlockServicesAsync(PriceBlockServices priceBlockServices)
        {
            _dbContext.PriceBlockServices.Update(priceBlockServices);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePriceBlockServicesAsync(int id)
        {
            var priceBlockService = await _dbContext.PriceBlockServices.FindAsync(id);
            if (priceBlockService == null)
            {
                throw new ArgumentException($"PriceBlockService with id {id} does not exist.");
            }

            _dbContext.PriceBlockServices.Remove(priceBlockService);
            await _dbContext.SaveChangesAsync();
        }

    }
}
