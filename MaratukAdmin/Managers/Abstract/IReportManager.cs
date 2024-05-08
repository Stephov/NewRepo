using MaratukAdmin.Entities.Report;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IReportManager
    {
        Task<List<ReportFlightInfo>> GetReportFlightInfo();
        //Task<ReportFlightInfo> GetReportFlightInfo();
        //Task<BookUniqueDepartureDatesByFlights> GetBookUniqueDepartureDates();
        Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates();
        Task<List<FlightReportPreparedData>> GetFlightReportPreparedData();

    }
}
