using AutoMapper;
using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;

namespace MaratukAdmin.Managers.Concrete
{
    public class PricePackageManager : IPricePackageManager
    {
        private readonly IMainRepository<PricePackage> _mainRepository;
        private readonly IMapper _mapper;


        public PricePackageManager(IMainRepository<PricePackage> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public  async Task<PricePackage> AddPricePackageAsync(AddPricePackage pricePackage)
        {
            var entity = _mapper.Map<PricePackage>(pricePackage);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeletePricePackageAsync(int id)
        {
            var entity = await _mainRepository.GetAsync(id);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound, "Entity not found");
            }

            bool result = await _mainRepository.DeleteAsync(id);
            return result;
        }

        public async  Task<List<PricePackage>> GetAllPricePackagesAsync()
        {
            return await _mainRepository.GetAllAsync();
        }

        public async  Task<PricePackage> GetPricePackageByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<PricePackage> UpdatePricePackageAsync(UpdatePricePackage pricePackage)
        {
            var entity = _mapper.Map<PricePackage>(pricePackage);

            await _mainRepository.UpdateAsync(entity);
            return entity;
        }
    }
}
