using AutoMapper.Internal;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Report;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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
        public async Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates(enumFlightReportType reportType, string flightNumber)
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

        public async Task<List<ReportFlightInfoPreparedData>> GetFlightReportPreparedData(enumFlightReportType reportType, string flightNumber)
        {
            var query = from dbf in _dbContext.BookedFlights
                        join f in _dbContext.Flight on (reportType == enumFlightReportType.Departure) ? dbf.StartFlightId : dbf.EndFlightId equals f.Id
                        join mas in _dbContext.MaratukAgentStatus on dbf.BookStatusForMaratuk equals mas.Id
                        where ((reportType == enumFlightReportType.Departure) ? dbf.TourStartDate != null : dbf.TourEndDate != null)
                                && f.FlightValue == (flightNumber == null ? f.FlightValue : flightNumber)
                        group new { mas, dbf, f } by new { mas.Id, mas.Name, dbf.TourStartDate.Date, f.FlightValue, dbf.StartFlightId, dbf.Rate } into grouped
                        orderby grouped.Key.Date
                        select new ReportFlightInfoPreparedData
                        {
                            FlightDate = grouped.Key.Date,
                            FlightNumber = grouped.Key.FlightValue,
                            StartFlightId = grouped.Key.StartFlightId,
                            MaratukAgentStatusId = grouped.Key.Id,
                            MaratukAgentStatusName = grouped.Key.Name,
                            StatusesCount = grouped.Count(),
                            TotalPrice = grouped.Sum(x => x.dbf.TotalPrice),
                            Currency = grouped.Key.Rate,
                            PassengersCount = grouped.Count(x => (reportType == enumFlightReportType.Departure) ? x.dbf.StartFlightId != null : x.dbf.EndFlightId != null)
                        };

            List<ReportFlightInfoPreparedData> flightReportDataList = await query.ToListAsync();

            return flightReportDataList;
        }

        //public async Task<List<ReportTouristInfoHotel>> GetTouristInfoPreparedDataAsync(enumTouristReportType reportType)
        public async Task<List<T>?> GetTouristInfoPreparedDataAsync<T>(enumTouristReportType reportType, DateTime? orderDateFrom = null, DateTime? orderDateTo = null) where T : class
        {
            List<ReportTouristInfoHotel> touristReportDataList = new();

            try
            {
                switch (reportType)
                {
                    case enumTouristReportType.Flight:
                        {
                            enumTourType tourType = enumTourType.Flight;
                            FieldInfo fieldInfo = typeof(enumTourType).GetField(tourType.ToString());
                            EnumMemberAttribute enumMemberAttribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();
                            string tourTypeString = enumMemberAttribute.Value;

                            var reportFlight = //await (
                                                      from bf in _dbContext.BookedFlights
                                                      join u in _dbContext.Users on bf.MaratukFlightAgentId equals u.Id
                                                      join pt in _dbContext.PassengerTypes on bf.PassengerTypeId equals pt.Id
                                                      join bsc in _dbContext.AgentStatus on bf.BookStatusForClient equals bsc.Id
                                                      join bsm in _dbContext.MaratukAgentStatus on bf.BookStatusForMaratuk equals bsm.Id
                                                      join sh in _dbContext.Schedule on bf.StartFlightId equals sh.FlightId
                                                      join shd1 in _dbContext.Schedule on bf.EndFlightId equals shd1.FlightId into gj
                                                      from subsh1 in gj.DefaultIfEmpty()
                                                      join depFlight in _dbContext.Flight on bf.StartFlightId equals depFlight.Id
                                                      join arrFlight in _dbContext.Flight on bf.EndFlightId equals arrFlight.Id into arrFlightGroup
                                                      from arrFlight in arrFlightGroup.DefaultIfEmpty()
                                                      join depCountry in _dbContext.Country on depFlight.DepartureCountryId equals depCountry.Id into depCountryGroup
                                                      from depCountry in depCountryGroup.DefaultIfEmpty()
                                                      join arrCountry in _dbContext.Country on arrFlight.DepartureCountryId equals arrCountry.Id into arrCountryGroup
                                                      from arrCountry in arrCountryGroup.DefaultIfEmpty()
                                                      join depCity in _dbContext.City on depFlight.DepartureCityId equals depCity.Id into depCityGroup
                                                      from depCity in depCityGroup.DefaultIfEmpty()
                                                      join arrCity in _dbContext.City on arrFlight.DepartureCityId equals arrCity.Id into arrCityGroup
                                                      from arrCity in arrCityGroup.DefaultIfEmpty()
                                                      join depAirport in _dbContext.Airport on depFlight.DepartureAirportId equals depAirport.Id into depAirportGroup
                                                      from depAirport in depAirportGroup.DefaultIfEmpty()
                                                      join arrAirport in _dbContext.Airport on arrFlight.DepartureAirportId equals arrAirport.Id into arrAirportGroup
                                                      from arrAirport in arrAirportGroup.DefaultIfEmpty()

                                                      where bf.ToureTypeId == tourTypeString
                                                            && (
                                                                bf.DateOfOrder.Date >= (orderDateFrom == null ? bf.DateOfOrder.Date : orderDateFrom)
                                                                && bf.DateOfOrder.Date <= (orderDateTo == null ? bf.DateOfOrder.Date : orderDateTo)
                                                                )
                                                      //orderby bf.DateOfOrder
                                                      select new ReportTouristInfoFlight()
                                                      {
                                                          OrderNumber = bf.OrderNumber,
                                                          ToureTypeId = bf.ToureTypeId,
                                                          Date = bf.DateOfOrder,
                                                          SurName = bf.Surname,
                                                          Name = bf.Name,
                                                          Manager = u.Name,
                                                          BookStatus = bsm.Name,
                                                          DepartureDate = bf.TourStartDate,
                                                          ArrivalDate = bf.TourEndDate,
                                                          DepartureTime = sh.DepartureTime != null ? sh.DepartureTime.ToString("HH:mm") : string.Empty,
                                                          ArrivalTime = sh.ArrivalTime != null ? sh.ArrivalTime.ToString("HH:mm") : string.Empty,
                                                          EndFlightDepartureTime = subsh1 != null && subsh1.DepartureTime != null ? subsh1.DepartureTime.ToString("HH:mm") : string.Empty,
                                                          EndFlightArrivalTime = subsh1 != null && subsh1.ArrivalTime != null ? subsh1.ArrivalTime.ToString("HH:mm") : string.Empty,
                                                          DepartureFlightValue = depFlight.FlightValue,
                                                          ArrivalFlightValue = arrFlight.FlightValue,
                                                          DepartureCountryName = depCountry.NameENG,
                                                          ArrivalCountryName = arrCountry.NameENG,
                                                          DepartureCityName = depCity.NameEng,
                                                          ArrivalCityName = arrCity.NameEng,
                                                          DepartureAirportName = depAirport.Name,
                                                          ArrivalAirportName = arrAirport.Name,
                                                          Tiitle = pt.TypeDescription,
                                                          Dob = bf.BirthDay,
                                                          Paid = bf.Paid,
                                                          TotalPrice = bf.TotalPrice,
                                                          Currency = bf.Rate
                                                      }
                                                      //).ToListAsync()
                                                      ;

                            var result = await reportFlight.OrderBy(c => c.Date).ToListAsync();
                            //var result = reportFlight;

                            return result as List<T>;
                        }
                    case enumTouristReportType.Hotel:
                        {
                            enumTourType tourType = enumTourType.FlightAndHotel;
                            FieldInfo fieldInfo = typeof(enumTourType).GetField(tourType.ToString());
                            EnumMemberAttribute enumMemberAttribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();
                            string tourTypeString = enumMemberAttribute.Value;

                            var reportHotel = //await (
                                                     from bf in _dbContext.BookedFlights
                                                     join bh in _dbContext.BookedHotel on bf.OrderNumber equals bh.OrderNumber
                                                     join bhg in _dbContext.BookedHotelGuest on bf.OrderNumber equals bhg.OrderNumber
                                                     join u in _dbContext.Users on bf.MaratukHotelAgentId equals u.Id
                                                     join pt in _dbContext.PassengerTypes on bf.PassengerTypeId equals pt.Id
                                                     join bsc in _dbContext.AgentStatus on bf.BookStatusForClient equals bsc.Id
                                                     join bsm in _dbContext.MaratukAgentStatus on bf.BookStatusForMaratuk equals bsm.Id
                                                     join sh in _dbContext.Schedule on bf.StartFlightId equals sh.FlightId into startFlights
                                                     from startFlight in startFlights.DefaultIfEmpty()
                                                     join sh1 in _dbContext.Schedule on bf.EndFlightId equals sh1.FlightId into endFlights
                                                     from endFlight in endFlights.DefaultIfEmpty()
                                                     join depFlight in _dbContext.Flight on bf.StartFlightId equals depFlight.Id
                                                     join arrFlight in _dbContext.Flight on bf.EndFlightId equals arrFlight.Id into arrFlightGroup
                                                     from arrFlight in arrFlightGroup.DefaultIfEmpty()
                                                     join depCountry in _dbContext.Country on depFlight.DepartureCountryId equals depCountry.Id into depCountryGroup
                                                     from depCountry in depCountryGroup.DefaultIfEmpty()
                                                     join arrCountry in _dbContext.Country on arrFlight.DepartureCountryId equals arrCountry.Id into arrCountryGroup
                                                     from arrCountry in arrCountryGroup.DefaultIfEmpty()
                                                     join depCity in _dbContext.City on depFlight.DepartureCityId equals depCity.Id into depCityGroup
                                                     from depCity in depCityGroup.DefaultIfEmpty()
                                                     join arrCity in _dbContext.City on arrFlight.DepartureCityId equals arrCity.Id into arrCityGroup
                                                     from arrCity in arrCityGroup.DefaultIfEmpty()
                                                     join depAirport in _dbContext.Airport on depFlight.DepartureAirportId equals depAirport.Id into depAirportGroup
                                                     from depAirport in depAirportGroup.DefaultIfEmpty()
                                                     join arrAirport in _dbContext.Airport on arrFlight.DepartureAirportId equals arrAirport.Id into arrAirportGroup
                                                     from arrAirport in arrAirportGroup.DefaultIfEmpty()
                                                     where bf.ToureTypeId == tourTypeString
                                                            && (
                                                                bf.DateOfOrder.Date >= (orderDateFrom == null ? bf.DateOfOrder.Date : orderDateFrom)
                                                                && bf.DateOfOrder.Date <= (orderDateTo == null ? bf.DateOfOrder.Date : orderDateTo)
                                                                )
                                                     //orderby bf.DateOfOrder
                                                     select new ReportTouristInfoFlight()
                                                     {
                                                         OrderNumber = bf.OrderNumber,
                                                         ToureTypeId = bf.ToureTypeId,
                                                         Date = bf.DateOfOrder,
                                                         SurName = bhg.Surname,
                                                         Name = bhg.Name,
                                                         Manager = u.Name,
                                                         BookStatus = bsm.Name,
                                                         DepartureDate = bf.TourStartDate,
                                                         ArrivalDate = bf.TourEndDate,
                                                         DepartureTime = string.Empty,
                                                         ArrivalTime = string.Empty,
                                                         EndFlightDepartureTime = string.Empty,
                                                         EndFlightArrivalTime = string.Empty,
                                                         DepartureFlightValue = depFlight.FlightValue,
                                                         ArrivalFlightValue = arrFlight.FlightValue,
                                                         DepartureCountryName = depCountry.NameENG,
                                                         ArrivalCountryName = arrCountry.NameENG,
                                                         DepartureCityName = depCity.NameEng,
                                                         ArrivalCityName = arrCity.NameEng,
                                                         DepartureAirportName = depAirport.Name,
                                                         ArrivalAirportName = arrAirport.Name,
                                                         Tiitle = pt.TypeDescription,
                                                         Dob = bf.BirthDay,
                                                         Paid = bf.Paid,
                                                         TotalPrice = bf.TotalPrice,
                                                         Currency = bf.Rate
                                                     }
                                                     //).ToListAsync()
                                                     ;

                            var result = await reportHotel.OrderBy(c => c.Date).ToListAsync();
                            return result as List<T>;
                        }
                    case enumTouristReportType.Accountant:
                        {
                            enumTourType tourType = enumTourType.Flight;
                            FieldInfo fieldInfo = typeof(enumTourType).GetField(tourType.ToString());
                            EnumMemberAttribute enumMemberAttribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();
                            string tourTypeFlightString = enumMemberAttribute.Value;
                            string sss = tourTypeFlightString;

                            var reportFlight = from bf in _dbContext.BookedFlights
                                               join u in _dbContext.Users on bf.MaratukFlightAgentId equals u.Id
                                               join pt in _dbContext.PassengerTypes on bf.PassengerTypeId equals pt.Id
                                               join bsc in _dbContext.AgentStatus on bf.BookStatusForClient equals bsc.Id
                                               join bsm in _dbContext.MaratukAgentStatus on bf.BookStatusForMaratuk equals bsm.Id
                                               join sh in _dbContext.Schedule on bf.StartFlightId equals sh.FlightId
                                               join shd1 in _dbContext.Schedule on bf.EndFlightId equals shd1.FlightId into gj
                                               from subsh1 in gj.DefaultIfEmpty()
                                               join depFlight in _dbContext.Flight on bf.StartFlightId equals depFlight.Id
                                               join arrFlight in _dbContext.Flight on bf.EndFlightId equals arrFlight.Id into arrFlightGroup
                                               from arrFlight in arrFlightGroup.DefaultIfEmpty()
                                               join depCountry in _dbContext.Country on depFlight.DepartureCountryId equals depCountry.Id into depCountryGroup
                                               from depCountry in depCountryGroup.DefaultIfEmpty()
                                               join arrCountry in _dbContext.Country on arrFlight.DepartureCountryId equals arrCountry.Id into arrCountryGroup
                                               from arrCountry in arrCountryGroup.DefaultIfEmpty()
                                               join depCity in _dbContext.City on depFlight.DepartureCityId equals depCity.Id into depCityGroup
                                               from depCity in depCityGroup.DefaultIfEmpty()
                                               join arrCity in _dbContext.City on arrFlight.DepartureCityId equals arrCity.Id into arrCityGroup
                                               from arrCity in arrCityGroup.DefaultIfEmpty()
                                               join depAirport in _dbContext.Airport on depFlight.DepartureAirportId equals depAirport.Id into depAirportGroup
                                               from depAirport in depAirportGroup.DefaultIfEmpty()
                                               join arrAirport in _dbContext.Airport on arrFlight.DepartureAirportId equals arrAirport.Id into arrAirportGroup
                                               from arrAirport in arrAirportGroup.DefaultIfEmpty()
                                               where bf.ToureTypeId == tourTypeFlightString
                                                        && (
                                                            bf.DateOfOrder.Date >= (orderDateFrom == null ? bf.DateOfOrder.Date : orderDateFrom)
                                                            && bf.DateOfOrder.Date <= (orderDateTo == null ? bf.DateOfOrder.Date : orderDateTo)
                                                            )
                                               //orderby bf.DateOfOrder
                                               select new
                                               {
                                                   OrderNumber = bf.OrderNumber,
                                                   ToureTypeId = bf.ToureTypeId,
                                                   Date = bf.DateOfOrder,
                                                   SurName = bf.Surname,
                                                   Name = bf.Name,
                                                   Manager = u.Name,
                                                   BookStatus = bsm.Name,
                                                   DepartureDate = bf.TourStartDate,
                                                   ArrivalDate = bf.TourEndDate,
                                                   DepartureTime = (DateTime?)sh.DepartureTime,
                                                   ArrivalTime = (DateTime?)sh.ArrivalTime,
                                                   EndFlightDepartureTime = (DateTime?)subsh1.DepartureTime,
                                                   //EndFlightArrivalTime = subsh1 != null && subsh1.ArrivalTime != null ? subsh1.ArrivalTime.TimeOfDay.ToString("HH:mm") : null,
                                                   EndFlightArrivalTime = (DateTime?)subsh1.ArrivalTime,
                                                   DepartureFlightValue = depFlight.FlightValue,
                                                   ArrivalFlightValue = arrFlight.FlightValue,
                                                   DepartureCountryName = depCountry.NameENG,
                                                   ArrivalCountryName = arrCountry.NameENG,
                                                   DepartureCityName = depCity.NameEng,
                                                   ArrivalCityName = arrCity.NameEng,
                                                   DepartureAirportName = depAirport.Name,
                                                   ArrivalAirportName = arrAirport.Name,
                                                   Tiitle = pt.TypeDescription,
                                                   Dob = bf.BirthDay,
                                                   Paid = bf.Paid,
                                                   TotalPrice = bf.TotalPrice,
                                                   Currency = bf.Rate
                                               };
                            tourType = enumTourType.FlightAndHotel;
                            fieldInfo = typeof(enumTourType).GetField(tourType.ToString());
                            enumMemberAttribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();
                            string tourTypeHotelString = enumMemberAttribute.Value;
                            string sss1 = tourTypeHotelString;
                            var reportHotel = from bf in _dbContext.BookedFlights
                                              join bh in _dbContext.BookedHotel on bf.OrderNumber equals bh.OrderNumber
                                              //join bhg in _dbContext.BookedHotelGuest on bf.OrderNumber equals bhg.OrderNumber        // this join makes duplicates
                                              join u in _dbContext.Users on bf.MaratukHotelAgentId equals u.Id
                                              join pt in _dbContext.PassengerTypes on bf.PassengerTypeId equals pt.Id
                                              join bsc in _dbContext.AgentStatus on bf.BookStatusForClient equals bsc.Id
                                              join bsm in _dbContext.MaratukAgentStatus on bf.BookStatusForMaratuk equals bsm.Id
                                              join sh in _dbContext.Schedule on bf.StartFlightId equals sh.FlightId into startFlights
                                              from startFlight in startFlights.DefaultIfEmpty()
                                              join sh1 in _dbContext.Schedule on bf.EndFlightId equals sh1.FlightId into endFlights
                                              from endFlight in endFlights.DefaultIfEmpty()
                                              join depFlight in _dbContext.Flight on bf.StartFlightId equals depFlight.Id
                                              join arrFlight in _dbContext.Flight on bf.EndFlightId equals arrFlight.Id into arrFlightGroup
                                              from arrFlight in arrFlightGroup.DefaultIfEmpty()
                                              join depCountry in _dbContext.Country on depFlight.DepartureCountryId equals depCountry.Id into depCountryGroup
                                              from depCountry in depCountryGroup.DefaultIfEmpty()
                                              join arrCountry in _dbContext.Country on arrFlight.DepartureCountryId equals arrCountry.Id into arrCountryGroup
                                              from arrCountry in arrCountryGroup.DefaultIfEmpty()
                                              join depCity in _dbContext.City on depFlight.DepartureCityId equals depCity.Id into depCityGroup
                                              from depCity in depCityGroup.DefaultIfEmpty()
                                              join arrCity in _dbContext.City on arrFlight.DepartureCityId equals arrCity.Id into arrCityGroup
                                              from arrCity in arrCityGroup.DefaultIfEmpty()
                                              join depAirport in _dbContext.Airport on depFlight.DepartureAirportId equals depAirport.Id into depAirportGroup
                                              from depAirport in depAirportGroup.DefaultIfEmpty()
                                              join arrAirport in _dbContext.Airport on arrFlight.DepartureAirportId equals arrAirport.Id into arrAirportGroup
                                              from arrAirport in arrAirportGroup.DefaultIfEmpty()
                                              where bf.ToureTypeId == tourTypeHotelString
                                                    && (
                                                        bf.DateOfOrder.Date >= (orderDateFrom == null ? bf.DateOfOrder.Date : orderDateFrom)
                                                        && bf.DateOfOrder.Date <= (orderDateTo == null ? bf.DateOfOrder.Date : orderDateTo)
                                                        )
                                              //orderby bf.DateOfOrder
                                              select new
                                              {
                                                  OrderNumber = bf.OrderNumber,
                                                  ToureTypeId = bf.ToureTypeId,
                                                  Date = bf.DateOfOrder,
                                                  //SurName = bhg.Surname,          // use name and surname from BookedFlights
                                                  //Name = bhg.Name,
                                                  SurName = bf.Surname,
                                                  Name = bf.Name,
                                                  Manager = u.Name,
                                                  BookStatus = bsm.Name,
                                                  DepartureDate = bf.TourStartDate,
                                                  ArrivalDate = bf.TourEndDate,
                                                  DepartureTime = (DateTime?)null,
                                                  ArrivalTime = (DateTime?)null,
                                                  EndFlightDepartureTime = (DateTime?)null,
                                                  EndFlightArrivalTime = (DateTime?)null,
                                                  DepartureFlightValue = depFlight.FlightValue,
                                                  ArrivalFlightValue = arrFlight.FlightValue,
                                                  DepartureCountryName = depCountry.NameENG,
                                                  ArrivalCountryName = arrCountry.NameENG,
                                                  DepartureCityName = depCity.NameEng,
                                                  ArrivalCityName = arrCity.NameEng,
                                                  DepartureAirportName = depAirport.Name,
                                                  ArrivalAirportName = arrAirport.Name,
                                                  Tiitle = pt.TypeDescription,
                                                  Dob = bf.BirthDay,
                                                  Paid = bf.Paid,
                                                  TotalPrice = bf.TotalPrice,
                                                  Currency = bf.Rate
                                              };

                            // *** UNION ALL ***
                            var combinedQuery = reportFlight.Union(reportHotel)
                                .Select(x => new ReportTouristInfoFlight
                                {
                                    OrderNumber = x.OrderNumber,
                                    ToureTypeId = x.ToureTypeId,
                                    Date = x.Date,
                                    SurName = x.SurName,
                                    Name = x.Name,
                                    Manager = x.Manager,
                                    BookStatus = x.BookStatus,
                                    DepartureDate = x.DepartureDate,
                                    ArrivalDate = x.ArrivalDate,
                                    DepartureTime = x.DepartureTime != null ? ((DateTime)x.DepartureTime).ToString(@"HH:mm") : string.Empty,
                                    ArrivalTime = x.ArrivalTime != null ? ((DateTime)x.ArrivalTime).ToString(@"HH:mm") : string.Empty,
                                    EndFlightDepartureTime = x.EndFlightDepartureTime != null ? ((DateTime)x.EndFlightDepartureTime).ToString(@"HH:mm") : string.Empty,
                                    EndFlightArrivalTime = x.EndFlightArrivalTime != null ? ((DateTime)x.EndFlightArrivalTime).ToString(@"HH:mm") : string.Empty,

                                    DepartureFlightValue = x.DepartureFlightValue,
                                    ArrivalFlightValue = x.ArrivalFlightValue,
                                    DepartureCountryName = x.DepartureCountryName,
                                    ArrivalCountryName = x.ArrivalCountryName,
                                    DepartureCityName = x.DepartureCityName,
                                    ArrivalCityName = x.ArrivalCityName,
                                    DepartureAirportName = x.DepartureAirportName,
                                    ArrivalAirportName = x.ArrivalAirportName,

                                    Tiitle = x.Tiitle,
                                    Dob = x.Dob,
                                    Paid = x.Paid,
                                    TotalPrice = x.TotalPrice,
                                    Currency = x.Currency
                                });

                            //var resultList = await combinedQuery.OrderBy(c => c.Date).OrderBy(c1 => c1.ToureTypeId).ToListAsync();
                            var resultList = await combinedQuery.OrderBy(c => c.Date).ToListAsync();

                            //******************

                            return resultList as List<T>;
                        }
                    default:
                        throw new ArgumentException("Invalid report type", nameof(reportType));
                }
            }
            catch (Exception)
            {
                throw;
            }
            //return touristReportDataList;
        }

        public async Task<List<T>?> GetReportAgencyDebtsAsync<T>(DateTime? dateFrom = null, DateTime? dateTo = null) where T : class
        {
            List<ReportAgencyDebts> agencyDebtsList = new();

            try
            {
                //var query =
                //            from dbf in _dbContext.BookedFlights
                //            join f in _dbContext.Flight on dbf.StartFlightId equals f.Id
                //            join mas in _dbContext.MaratukAgentStatus on dbf.BookStatusForMaratuk equals mas.Id
                //            join au in _dbContext.AgencyUser on dbf.AgentId equals au.Id
                //            where dbf.TourStartDate != null
                //            group dbf by new
                //            {
                //                dbf.AgentId,
                //                dbf.TourStartDate,
                //                f.FlightValue,
                //                au.FullCompanyName,
                //                dbf.Rate
                //            } into grouped
                //            orderby grouped.Key.TourStartDate, grouped.Key.AgentId
                //            select new ReportAgencyDebts()
                //            {
                //                FlightDate = grouped.Key.TourStartDate.Date,
                //                FlightNumber = grouped.Key.FlightValue,
                //                AgencyName = grouped.Key.FullCompanyName,
                //                Currency = grouped.Key.Rate,
                //                Debt = grouped.Sum(g => g.Dept),
                //                Paid = grouped.Sum(g => g.Paid),
                //                PaidAMD = grouped.Sum(g => g.Paid),
                //                TotalAmount = grouped.Sum(g => g.TotalPrice)
                //            };
                var query =
                            from dbf in _dbContext.BookedFlights
                            join f in _dbContext.Flight on dbf.StartFlightId equals f.Id
                            join mas in _dbContext.MaratukAgentStatus on dbf.BookStatusForMaratuk equals mas.Id
                            join au in _dbContext.AgencyUser on dbf.AgentId equals au.Id
                            where dbf.TourStartDate != null &&
                                  dbf.DateOfOrder >= dateFrom &&
                                  dbf.DateOfOrder <= dateTo
                            group new { dbf, f, au } by new
                            {
                                dbf.TourStartDate,
                                f.FlightValue
                            } into flightGroup
                            select new ReportAgencyDebts
                            {
                                FlightDate = flightGroup.Key.TourStartDate,
                                FlightNumber = flightGroup.Key.FlightValue,
                                AgencyDebts = flightGroup.GroupBy(g => new
                                {
                                    g.au.FullCompanyName,
                                    g.dbf.Rate
                                })
                                .Select(agencyGroup => new AgencyDebtDetails
                                {
                                    AgencyName = agencyGroup.Key.FullCompanyName,
                                    Currency = agencyGroup.Key.Rate,
                                    Debt = agencyGroup.Sum(g => g.dbf.Dept),
                                    Paid = agencyGroup.Sum(g => g.dbf.Paid),
                                    TotalAmount = agencyGroup.Sum(g => g.dbf.TotalPrice)
                                }).ToList()
                            };

                var result = await query.OrderBy(c => c.FlightDate).ThenBy(c => c.FlightNumber).ToListAsync();


                return result as List<T>;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<T>?> GetSalesByManagersPreparedDataAsync<T>(DateTime? orderDateFrom = null, DateTime? orderDateTo = null, enumBookStatusForMaratuk bookStatus = enumBookStatusForMaratuk.All) where T : class
        {
            List<ReportSalesByManagerPreparedData> sales = new();
            try
            {
                var query = from dbf in _dbContext.BookedFlights
                            join mas in _dbContext.MaratukAgentStatus on dbf.BookStatusForMaratuk equals mas.Id
                            join au in _dbContext.AgencyUser on dbf.AgentId equals au.Id
                            join u in _dbContext.Users on dbf.MaratukFlightAgentId equals u.Id
                            join u1 in _dbContext.Users on dbf.MaratukHotelAgentId equals u1.Id into uGroup
                            from u1 in uGroup.DefaultIfEmpty()
                            join f1 in _dbContext.Flight on dbf.StartFlightId equals f1.Id
                            join bh in _dbContext.BookedHotel on dbf.OrderNumber equals bh.OrderNumber into bhGroup
                            from bh in bhGroup.DefaultIfEmpty()
                            join h in _dbContext.Hotel on bh.HotelCode equals h.Code into hGroup
                            from h in hGroup.DefaultIfEmpty()
                            join f2 in _dbContext.Flight on dbf.EndFlightId equals f2.Id into f2Group
                            from f2 in f2Group.DefaultIfEmpty()
                            where dbf.TourStartDate != null
                            && (
                                dbf.DateOfOrder.Date >= (orderDateFrom == null ? dbf.DateOfOrder.Date : orderDateFrom)
                                && dbf.DateOfOrder.Date <= (orderDateTo == null ? dbf.DateOfOrder.Date : orderDateTo)
                                )
                            && mas.Id == (bookStatus == enumBookStatusForMaratuk.All ? mas.Id : (int)bookStatus)
                            select new ReportSalesByManagerPreparedData()
                            {
                                Date = dbf.TourStartDate.Date,
                                OrderNumber = dbf.OrderNumber,
                                BookStatus = mas.Name,
                                AgencyName = au.FullCompanyName,
                                PassengerName = dbf.Name,
                                PassengerSurName = dbf.Surname,
                                PassengersCount = dbf.PassengersCount,
                                HotelName = h.Name,
                                TicketsCostTotal = dbf.TotalPrice,
                                Rate = dbf.Rate,
                                TicketsCostInAMD = dbf.TotalPriceAmd,
                                HotelCostInAMD = bh.HotelTotalPriceAmd,
                                TourStartDate = dbf.TourStartDate,
                                TourEndDate = dbf.TourEndDate,
                                Direction1 = f1.Name,
                                Direction2 = f2.Name,
                                FlightManagerName = u.Name,
                                HotelManagerName = u1.Name,
                                TicketsCount = 1 // as a constant value
                            };


                var result = await query.OrderBy(c => c.Date).ToListAsync();

                return result as List<T>;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<T>?> GetReportTotalPreparedDataAsync<T>(DateTime? orderDateFrom = null, DateTime? orderDateTo = null, enumBookStatusForMaratuk bookStatus = enumBookStatusForMaratuk.All) where T : class
        {
            try
            {
                var query = from dbf in _dbContext.BookedFlights
                            join mas in _dbContext.MaratukAgentStatus on dbf.BookStatusForMaratuk equals mas.Id
                            join au in _dbContext.AgencyUser on dbf.AgentId equals au.Id
                            join u in _dbContext.Users on dbf.MaratukFlightAgentId equals u.Id
                            join f1 in _dbContext.Flight on dbf.StartFlightId equals f1.Id
                            join s1 in _dbContext.Schedule on dbf.StartFlightId equals s1.FlightId
                            join bh in _dbContext.BookedHotel on dbf.OrderNumber equals bh.OrderNumber into bhGroup
                            from bh in bhGroup.DefaultIfEmpty()
                            join h in _dbContext.Hotel on bh.HotelCode equals h.Code into hGroup
                            from h in hGroup.DefaultIfEmpty()
                            join f2 in _dbContext.Flight on dbf.EndFlightId equals f2.Id into f2Group
                            from f2 in f2Group.DefaultIfEmpty()
                            join s2 in _dbContext.Schedule on dbf.EndFlightId equals s2.FlightId into s2Group
                            from s2 in s2Group.DefaultIfEmpty()
                            where dbf.TourStartDate != null
                                && dbf.ToureTypeId == "Flight + Hotel"
                                && (
                                    dbf.DateOfOrder.Date >= (orderDateFrom == null ? dbf.DateOfOrder.Date : orderDateFrom)
                                    && dbf.DateOfOrder.Date <= (orderDateTo == null ? dbf.DateOfOrder.Date : orderDateTo)
                                    )
                                && mas.Id == (bookStatus == enumBookStatusForMaratuk.All ? mas.Id : (int)bookStatus)
                            orderby dbf.TourStartDate, dbf.AgentId
                            select new ReportTotalPreparedData()
                            {
                                DateOfOrder = dbf.DateOfOrder,
                                OrderNumber = dbf.OrderNumber,
                                TourManager = u.Name,
                                CompanyName = au.FullCompanyName,
                                PassengerName = dbf.Name,
                                PassengerSurName = dbf.Surname,
                                FlightStartDate = s1.FlightStartDate,
                                DepartureTime = s1.DepartureTime,
                                FlightEndDate = s2.FlightEndDate,
                                ArrivalTime = s2.ArrivalTime,
                                BookStatus = mas.Name,
                                //RoomPrice 
                                AccomodationDaysCount = bh.AccomodationDaysCount ?? 0,
                                Rate = dbf.Rate,
                                HotelTotal = (bh.HotelTotalPrice == null ? 0 : bh.HotelTotalPrice),
                                HotelTotalAMD = (bh.HotelTotalPriceAmd == null ? 0 : bh.HotelTotalPriceAmd)
                            };

                var result = await query.OrderBy(c => c.DateOfOrder).ToListAsync();

                return result as List<T>;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
