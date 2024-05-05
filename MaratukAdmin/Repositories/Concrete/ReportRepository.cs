using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Report;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class ReportRepository : IReportRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public ReportRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //public async Task<BookUniqueDepartureDatesByFlights> GetBookUniqueDepartureDates()
        public async Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates()
        {
            //var uniqueDepartureDates = _dbContext.BookedFlights
            //    .Where(bf => bf.TourEndDate != null)
            //    .Select(bf => ((DateTime)bf.TourEndDate).Date)
            //    .Distinct();

            //var uniqueDepartureDates = (from bf in _dbContext.BookedFlights
            // //join bp in _dbContext.BookPayments on bf.OrderNumber equals bp.OrderNumber
            // where bf.TourStartDate != null
            // select bf.TourStartDate.Date)
            // .Distinct();

            //BookUniqueDepartureDatesByFlights result = new()
            //{
            //    DepartureDate = await uniqueDepartureDates.Select(date => new DateOnly(date.Year, date.Month, date.Day)).ToListAsync()
            //};

            var result = await (from bf in _dbContext.BookedFlights
                        join f in _dbContext.Flight on bf.StartFlightId equals f.Id
                        where bf.TourStartDate != null
                        orderby bf.TourStartDate
                        select new BookUniqueDepartureDatesByFlights
                        {
                            DepartureDate = bf.TourStartDate.Date,
                            FlightNumber = f.FlightValue
                        })
                        .Distinct()
                        .ToListAsync();

            //List<BookUniqueDepartureDatesByFlights> result = await query.ToListAsync();


            return result;
        }

        public async Task<List<FlightReportPreparedData>> GetFlightReportPreparedData()
        {
            var query = from dbf in _dbContext.BookedFlights
                        join f in _dbContext.Flight on dbf.StartFlightId equals f.Id
                        join mas in _dbContext.MaratukAgentStatus on dbf.BookStatusForMaratuk equals mas.Id
                        where dbf.TourStartDate != null
                        group new { mas, dbf, f } by new { mas.Id, mas.Name, dbf.TourStartDate.Date, f.FlightValue, dbf.StartFlightId, dbf.Rate } into grouped
                        orderby grouped.Key.Date
                        select new FlightReportPreparedData
                        {
                            DepartureDate = grouped.Key.Date,
                            FlightNumber = grouped.Key.FlightValue,
                            StartFlightId = grouped.Key.StartFlightId,
                            MaratukAgentStatusId = grouped.Key.Id,
                            MaratukAgentStatusName = grouped.Key.Name,
                            StatusesCount = grouped.Count(),
                            TotalPrice = grouped.Sum(x => x.dbf.TotalPrice),
                            Currency = grouped.Key.Rate,
                            PassengersCount = grouped.Count(x => x.dbf.StartFlightId != null)
                        };

            List<FlightReportPreparedData> flightReportDataList = await query.ToListAsync();

            return flightReportDataList;
        }
        

    }
}
