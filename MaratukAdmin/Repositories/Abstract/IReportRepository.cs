using MaratukAdmin.Entities.Report;
using static MaratukAdmin.Utils.Enums;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IReportRepository 
    {
        //Task<BookUniqueDepartureDatesByFlights> GetBookUniqueDepartureDates();
        Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates();
        Task<List<FlightReportPreparedData>> GetFlightReportPreparedData();
        //Task<List<ReportTouristInfoHotel>> GetTouristInfoPreparedData(enumTouristReportType reportType);
        Task<List<T>?> GetTouristInfoPreparedDataAsync<T>(enumTouristReportType reportType) where T : class;
    }
}
