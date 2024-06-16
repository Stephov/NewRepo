using MaratukAdmin.Entities.Report;
using static MaratukAdmin.Utils.Enums;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IReportManager
    {
        Task<List<ReportFlightInfo>> GetReportFlightInfo(enumFlightReportType reportType, string flightNumber);
        //Task<ReportFlightInfo> GetReportFlightInfo();
        //Task<BookUniqueDepartureDatesByFlights> GetBookUniqueDepartureDates();
        Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates(enumFlightReportType reportType, string flightNumber);
        Task<List<FlightReportPreparedData>> GetFlightReportPreparedData(enumFlightReportType reportType, string flightNumber);
        //Task<List<ReportTouristInfoHotel>> GetReportTouristInfo(enumTouristReportType reportType, int priceBlockId);
        //Task<T> GetReportTouristInfoAsync<T>(enumTouristReportType reportType, int priceBlockId) where T : class;
        Task<List<T>?> GetReportTouristInfoAsync<T>(enumTouristReportType reportType, DateTime? orderDateFrom = null, DateTime? orderDateTo = null, bool includeRate = false) where T : class;
        Task<List<T>?> GetReportAgencyDebtsAsync<T>(DateTime? dateFrom = null, DateTime? dateTo = null) where T : class;

    }
}
