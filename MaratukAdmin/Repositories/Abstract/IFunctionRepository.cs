using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IFunctionRepository
    {
        Task<List<FlightInfoFunction>> GetFligthInfoFunctionAsync(int TripTypeId);
    }
}
