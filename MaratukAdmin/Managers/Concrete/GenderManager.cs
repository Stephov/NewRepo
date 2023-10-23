using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class GenderManager : IGenderManager
    {

        private readonly IMainRepository<Gender> _mainRepository;
        private readonly IMapper _mapper;


        public GenderManager(IMainRepository<Gender> mainRepository, IMapper mapper)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
        }

 

        public async Task<Gender> GetGenderNameByIdAsync(int id)
        {
           return await _mainRepository.GetAsync(id);
        }

        public async Task<List<Gender>> GetGenderAsync()
        {
            var result = await _mainRepository.GetAllAsync();
            return result;        
        }
    }
}
