using MaratukAdmin.Entities.Report;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IReportRepository 
    {
        //Task<BookUniqueDepartureDatesByFlights> GetBookUniqueDepartureDates();
        Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates();
        Task<List<FlightReportPreparedData>> GetFlightReportPreparedData();
    }
}
