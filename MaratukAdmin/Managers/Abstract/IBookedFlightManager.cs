using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IBookedFlightManager
    {
        Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn);
        Task<bool> AddBookedFlightAsync(List<AddBookedFlight> addBookedFlights);
        Task<BookedFlightResponseFinal> GetBookedFlightByAgentIdAsync(int id);
        Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightByMaratukAgentIdAsync(int maratukAgent, int pageNumber = 1, int pageSize = 10);
        Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightByMaratukAgentIdAsync(int maratukAgent, string? searchText,int pageNumber = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null);
        Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightAsync(int userId,int roleId, string? searchText,int pageNumber = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null);
        Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightForAccAsync(int pageNumber, int pageSize);
        Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightForAccAsync(int pageNumber, int pageSize,string? text,DateTime? startDate, DateTime? endDate);
        Task<bool> UpdateBookedStatusAsync(string orderNumber, int status,string comment);
        Task<BookedFlight> UpdateBookedUserInfoAsync(BookedUserInfoForMaratukRequest bookedUserInfoForMaratuk);
    }
}
