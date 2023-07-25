using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
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

            }
            catch (Exception ex)
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

        /*   public async Task<IEnumerable<PriceBlockServices>> GetPriceBlockServicesByPriceBlockIdAsync(int priceBlockId)
           {
               return await _dbContext.PriceBlockServices
                   .Where(s => s.PriceBlockId == priceBlockId)
                   .ToListAsync();
           }*/

        public async Task<PriceBlockServices> CreatePriceBlockServicesAsync(PriceBlockServices priceBlockServices)
        {
            try
            {
                await _dbContext.PriceBlockServices.AddAsync(priceBlockServices);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                string a = ex.Message;
            }

            return priceBlockServices;
        }


        public async Task<bool> DeletePriceBlockServicesAsync(int id)
        {
            var priceBlockService = await _dbContext.PriceBlockServices.FindAsync(id);
            if (priceBlockService != null)
            {
                _dbContext.PriceBlockServices.Remove(priceBlockService);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<PriceBlockServices>> GetServicesByPriceBlockIdAsync(int id)
        {
            return await _dbContext.PriceBlockServices
                 .Where(p => p.PriceBlockId == id)
                 .ToListAsync();
        }

        public async Task<ServicesPricingPolicy> CreateServicesPricingPolicyAsync(ServicesPricingPolicy servicesPricingPolicy)
        {
            try
            {
                await _dbContext.ServicesPricingPolicy.AddAsync(servicesPricingPolicy);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                string a = ex.Message;
            }

            return servicesPricingPolicy;
        }

        public async Task<ServicesPricingPolicy> GetServicesPricingPolicyByIdAsync(int id)
        {
            return await _dbContext.ServicesPricingPolicy
               .Where(p => p.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<List<ServicesPricingPolicy>> GetServicesPricingPolicyByPriceBlockServicesIdAsync(int id)
        {
            return await _dbContext.ServicesPricingPolicy
               .Where(p => p.PriceBlockServicesId == id)
               .ToListAsync();
        }

        public async Task<bool> DeleteServicesPricingPolicyAsync(int id)
        {
            var priceBlockService = await _dbContext.ServicesPricingPolicy.FindAsync(id);
            if (priceBlockService != null)
            {
                _dbContext.ServicesPricingPolicy.Remove(priceBlockService);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ServicesPricingPolicy> UpdateServicesPricingPolicyAsync(ServicesPricingPolicy servicesPricingPolicy)
        {
            _dbContext.ServicesPricingPolicy.Update(servicesPricingPolicy);
            await _dbContext.SaveChangesAsync();

            return servicesPricingPolicy;
        }
    }
}

