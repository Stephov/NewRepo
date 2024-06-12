using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IBookedFlightManager
    {
        Task<BookedFlightResponseFinal> GetBookedFlightAsync(int Itn);
        //Task<bool> AddBookedFlightAsync(List<AddBookedFlight> addBookedFlights);
        Task<bool> AddBookedFlightAsync(BookedFlightModel addBookedFlights);
        Task<BookedFlightResponseFinal> GetBookedFlightByAgentIdAsync(int id);
        Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightByMaratukAgentIdAsync(int roleId, int maratukAgent, int pageNumber = 1, int pageSize = 10);
        Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightByMaratukAgentIdAsync(int roleId, int maratukAgent, string? searchText, int? status, int pageNumber = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null);
        Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightAsync(int userId, int roleId, string? searchText, int? status, int pageNumber = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null);
        Task<BookedFlightResponseFinalForMaratukAgent> GetBookedFlightForAccAsync(int pageNumber, int pageSize);
        Task<BookedFlightResponseFinalForMaratukAgent> SearchBookedFlightForAccAsync(int pageNumber, int pageSize, string? text, int? status, DateTime? startDate, DateTime? endDate);
        Task<bool> UpdateBookedStatusAsync(string orderNumber, int status, int role, double? totalPrice, string comment);
        Task<BookedFlight> UpdateBookedUserInfoAsync(BookedUserInfoForMaratukRequest bookedUserInfoForMaratuk);
        Task<ReturnStatusResponse> SetTicketNumberToBookAsync(SetTicketNumberToBookRequest request);
    }
}
