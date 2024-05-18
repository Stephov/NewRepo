using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Report;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static MaratukAdmin.Utils.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        //public async Task<List<ReportTouristInfoHotel>> GetTouristInfoPreparedDataAsync(enumTouristReportType reportType)
        public async Task<List<T>?> GetTouristInfoPreparedDataAsync<T>(enumTouristReportType reportType) where T : class
        {
            //List<ReportTouristInfo> touristReportDataList = await query.ToListAsync();
            List<ReportTouristInfoHotel> touristReportDataList = new();

            try
            {
                switch (reportType)
                {
                    case enumTouristReportType.Flight:
                        {
                            //ReportTouristInfoFlight reportFlight = new();
                            var reportFlight = from bf in _dbContext.BookedFlights
                                               join u in _dbContext.Users on bf.MaratukFlightAgentId equals u.Id
                                               join pt in _dbContext.PassengerTypes on bf.PassengerTypeId equals pt.Id
                                               join bsc in _dbContext.AgentStatus on bf.BookStatusForClient equals bsc.Id
                                               join bsm in _dbContext.MaratukAgentStatus on bf.BookStatusForMaratuk equals bsm.Id
                                               join sh in _dbContext.Schedule on bf.StartFlightId equals sh.Id
                                               //left join sh1 in _db.Schedule on bf.EndFlightId equals sh1.Id into gj
                                               //from subsh in gj.DefaultIfEmpty()
                                               where bf.ToureTypeId == "Flight"
                                               select new ReportTouristInfoFlight()
                                               {
                                                   Date = bf.DateOfOrder,
                                                   SurName = bf.Surname,
                                                   Name = bf.Name,
                                                   FlightManager = u.Name,
                                                   BookStatus = bsm.Name,
                                                   DepartureDate = bf.TourStartDate,
                                                   DepartureTime = sh.DepartureTime,
                                                   ArrivalDate = bf.TourEndDate,
                                                   ArrivalTime = sh.ArrivalTime,
                                                   Tiitle = pt.TypeDescription,
                                                   Dob = bf.BirthDay,
                                                   Paid = bf.Paid,
                                                   Currency = bf.Rate,
                                                   // EndFlightTourStartDate = bf.TourStartDate,
                                                   // EndFlightDepartureTime = sh.DepartureTime,
                                                   // EndFlightTourEndDate = bf.TourEndDate,
                                                   // EndFlightArrivalTime = sh.ArrivalTime
                                               };

                            //var reportFlight = from bf in _dbContext.BookedFlights
                            //                   select new ReportTouristInfoFlight()
                            //                   {
                            //                       Dob = bf.BirthDay
                            //                   };

                            var result = await reportFlight.ToListAsync();


                            //return result as T;
                            return result as List<T>;
                        }

                    //case enumTouristReportType.Hotel:
                    //    var reportB = new ReportTouristInfoHotel { DataB = "Data for Report B" };
                    //    return await Task.FromResult(reportB as T);

                    default:
                        throw new ArgumentException("Invalid report type", nameof(reportType));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            //return touristReportDataList;
        }

    }
}
