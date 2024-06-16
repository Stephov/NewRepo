using MaratukAdmin.Entities.Report;
using static MaratukAdmin.Utils.Enums;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IReportRepository 
    {
        //Task<BookUniqueDepartureDatesByFlights> GetBookUniqueDepartureDates();
        Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates(enumFlightReportType reportType, string flightNumber);
        Task<List<FlightReportPreparedData>> GetFlightReportPreparedData(enumFlightReportType reportType, string flightNumber);
        //Task<List<ReportTouristInfoHotel>> GetTouristInfoPreparedData(enumTouristReportType reportType);
        Task<List<T>?> GetTouristInfoPreparedDataAsync<T>(enumTouristReportType reportType, DateTime? orderDateFrom = null, DateTime? orderDateTo = null) where T : class;
        Task<List<T>?> GetReportAgencyDebtsAsync<T>(DateTime? dateFrom = null, DateTime? dateTo = null)where T : class;
    }
}
