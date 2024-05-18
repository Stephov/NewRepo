using MaratukAdmin.Entities.Report;
using static MaratukAdmin.Utils.Enums;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IReportManager
    {
        Task<List<ReportFlightInfo>> GetReportFlightInfo();
        //Task<ReportFlightInfo> GetReportFlightInfo();
        //Task<BookUniqueDepartureDatesByFlights> GetBookUniqueDepartureDates();
        Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates();
        Task<List<FlightReportPreparedData>> GetFlightReportPreparedData();
        //Task<List<ReportTouristInfoHotel>> GetReportTouristInfo(enumTouristReportType reportType, int priceBlockId);
        //Task<T> GetReportTouristInfoAsync<T>(enumTouristReportType reportType, int priceBlockId) where T : class;
        Task<List<T>?> GetReportTouristInfoAsync<T>(enumTouristReportType reportType) where T : class;

    }
}
