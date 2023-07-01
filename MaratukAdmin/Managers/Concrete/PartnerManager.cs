using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class PartnerManager : IPartnerManager
    {

        private readonly IMainRepository<Partner> _mainRepository;
        private readonly IMapper _mapper;


        public PartnerManager(IMainRepository<Partner> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

        public async Task<Partner> AddPartnerAsync(AddPartner partner)
        {
            var entity = _mapper.Map<Partner>(partner);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<Partner> GetPartnerNameByIdAsync(int id)
        {
           return await _mainRepository.GetAsync(id);
        }

        public async Task<List<Partner>> GetPartnerAsync()
        {
            return await _mainRepository.GetAllAsync();
        }
    }
}
