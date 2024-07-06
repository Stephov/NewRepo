using Bogus.DataSets;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Report;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using static MaratukAdmin.Utils.Enums;

namespace MaratukAdmin.Managers.Concrete
{
    public class ReportManager : IReportManager
    {
        private readonly IReportRepository _reportRepository;
        private readonly IContractExportRepository _contractExportRepository;
        private readonly ICurrencyRatesManager _currencyRatesManager;

        public ReportManager(IReportRepository reportRepository,
                            IContractExportRepository contractExportRepository,
                            ICurrencyRatesManager currencyRatesManager)
        {
            _reportRepository = reportRepository;
            _contractExportRepository = contractExportRepository;
            _currencyRatesManager = currencyRatesManager;
        }



        public async Task<List<ReportFlightInfoPreparedData>> GetFlightReportPreparedData(enumFlightReportType reportType, string flightNumber)
        {
            return await _reportRepository.GetFlightReportPreparedData(reportType, flightNumber);
        }
        public async Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates(enumFlightReportType reportType, string flightNumber)
        {
            return await _reportRepository.GetBookUniqueDepartureDates(reportType, flightNumber);
        }
        public async Task<List<ReportFlightInfo>> GetReportFlightInfo(enumFlightReportType reportType, string flightNumber)
        {
            List<ReportFlightInfo> retValue = new();
            DateTime prevDepartureDate = DateTime.MinValue;
            string prevFlightNumber = "NoInfo";
            string prevCurrency = "NoInfo";
            int passengersCountTotal = -1;
            double totalPriceSum = -1;
            bool infoChanged = true;
            ReportFlightInfo newInfo = new();

            //var commission = _contractExportRepository.GetFlightCommission(308);

            List<ReportFlightInfoPreparedData> flightReportPreparedData = await GetFlightReportPreparedData(reportType, flightNumber);

            if (flightReportPreparedData != null)
            {
                foreach (var pData in flightReportPreparedData)
                {
                    if (prevDepartureDate == DateTime.MinValue || (prevDepartureDate != DateTime.MinValue && prevDepartureDate != pData.FlightDate)
                        || (prevFlightNumber == "NoInfo" || (prevFlightNumber != "NoInfo" && prevFlightNumber != pData.FlightNumber))
                        || (prevCurrency == "NoInfo" || (prevCurrency != "NoInfo" && prevCurrency != pData.Currency))
                        )
                    {
                        infoChanged = true;
                    }
                    else
                        infoChanged = false;

                    // Info changed
                    if (infoChanged)
                    {
                        passengersCountTotal = pData.PassengersCount;
                        totalPriceSum = pData.TotalPrice;

                        newInfo = new()
                        {
                            FlightDate = pData.FlightDate,
                            FlightNumber = pData.FlightNumber,
                            WaitingCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.Waiting ? pData.StatusesCount : 0,
                            ConfirmedCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.Confirmed ? pData.StatusesCount : 0,
                            ConfirmedByAccountantCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.ConfirmedByAccountant ? pData.StatusesCount : 0,
                            InvoiceSentCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.InvoiceSent ? pData.StatusesCount : 0,
                            PaidPartiallyCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.PaidPartially ? pData.StatusesCount : 0,
                            PaidFullCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.PaidInFull ? pData.StatusesCount : 0,
                            TicketSentCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.TicketSent ? pData.StatusesCount : 0,
                            TicketConfirmedCount = pData.MaratukAgentStatusId == (int)Enums.enumBookStatusForMaratuk.TicketIsConfirmed ? pData.StatusesCount : 0,
                            Currency = pData.Currency,
                            TotalPrice = totalPriceSum,
                            TotalPassCount = passengersCountTotal,
                        };
                    }
                    else
                    // Info NOT changed
                    {
                        passengersCountTotal += pData.PassengersCount;
                        totalPriceSum += pData.TotalPrice;

                        // DepartureDate - the same
                        // FlightNumber - the same
                        switch (pData.MaratukAgentStatusId)
                        {
                            case (int)Enums.enumBookStatusForMaratuk.Waiting:
                                newInfo.WaitingCount = pData.StatusesCount;
                                break;
                            case (int)Enums.enumBookStatusForMaratuk.Confirmed:
                                newInfo.ConfirmedCount = pData.StatusesCount;
                                break;
                            case (int)Enums.enumBookStatusForMaratuk.ConfirmedByAccountant:
                                newInfo.ConfirmedByAccountantCount = pData.StatusesCount;
                                break;
                            case (int)Enums.enumBookStatusForMaratuk.InvoiceSent:
                                newInfo.InvoiceSentCount = pData.StatusesCount;
                                break;
                            case (int)Enums.enumBookStatusForMaratuk.PaidPartially:
                                newInfo.PaidPartiallyCount = pData.StatusesCount;
                                break;
                            case (int)Enums.enumBookStatusForMaratuk.PaidInFull:
                                newInfo.PaidFullCount = pData.StatusesCount;
                                break;
                            case (int)Enums.enumBookStatusForMaratuk.TicketSent:
                                newInfo.TicketSentCount = pData.StatusesCount;
                                break;
                            case (int)Enums.enumBookStatusForMaratuk.TicketIsConfirmed:
                                newInfo.TicketConfirmedCount = pData.StatusesCount;
                                break;
                        }
                        newInfo.Currency = pData.Currency;
                        newInfo.TotalPrice = totalPriceSum;
                        newInfo.TotalPassCount = passengersCountTotal;
                    }

                    prevDepartureDate = pData.FlightDate;
                    prevFlightNumber = pData.FlightNumber;
                    prevCurrency = pData.Currency;

                    if (infoChanged)
                    {
                        retValue.Add(newInfo);
                    }
                    else
                    {
                        retValue[retValue.Count - 1] = newInfo;
                    }
                }
            }

            //List<ReportFlightInfo> retValue = new();
            //ReportFlightInfo item = new();

            //List<BookUniqueDepartureDatesByFlights> distinctDepartureDatesByFlights = await GetBookUniqueDepartureDates();

            //if (distinctDepartureDatesByFlights != null && distinctDepartureDatesByFlights.Count > 0)
            //{
            //    foreach (var item in distinctDepartureDatesByFlights)
            //    {
            //        retValue.DepartureDate = (DateTime)item.DepartureDate;
            //        retValue.FlightNumber = item.FlightNumber;
            //        //departureDate
            //    }
            //}

            return retValue;
        }

        public async Task<List<T>?> GetReportTouristInfoAsync<T>(enumTouristReportType reportType, DateTime? orderDateFrom = null, DateTime? orderDateTo = null, bool includeRate = false) where T : class
        {
            //var commission = await _contractExportRepository.GetFlightCommission(priceBlockId);

            //if (reportType == enumTouristReportType.Flight)
            //{
            var touristReportPreparedData = await _reportRepository.GetTouristInfoPreparedDataAsync<T>(reportType, orderDateFrom, orderDateTo);

            if (touristReportPreparedData != null && includeRate)
            {
                foreach (var item in touristReportPreparedData as List<ReportTouristInfoFlight>)
                {
                    var currRate = await _currencyRatesManager.GetCurrencyRatesAsync(item.Date, item.Currency);
                    item.CurrencyRate = (currRate == null || currRate.Count == 0) ? 0 : currRate.FirstOrDefault().OfficialRate;
                }
            }

            //}

            return touristReportPreparedData;
        }

        public async Task<List<T>?> GetReportAgencyDebtsAsync<T>(DateTime? dateFrom = null, DateTime? dateTo = null) where T : class
        {
            var agencyDebtsPreparedData = await _reportRepository.GetReportAgencyDebtsAsync<T>(dateFrom, dateTo);
            List<CurrencyRatesResponse> currencyRatesList = new();
            CurrencyRatesResponse? currRate = null;

            if (agencyDebtsPreparedData != null)
            {
                foreach (var baseItem in agencyDebtsPreparedData as List<ReportAgencyDebts>)
                {
                    foreach (var item in baseItem.AgencyDebts)
                    {

                        currRate = currencyRatesList.FirstOrDefault(c => c.CodeIso == item.Currency);

                        if (currRate == null)
                        {
                            var newRate = await _currencyRatesManager.GetCurrencyRatesAsync(baseItem.FlightDate, item.Currency);
                            currencyRatesList.AddRange(newRate);
                            currRate = currencyRatesList.FirstOrDefault(c => c.CodeIso == item.Currency);
                        }

                        item.PaidAMD = (currRate == null) ? 0 : (item.Paid * currRate.OfficialRate);
                        item.PaidAMD = (double)Math.Ceiling((double)item.PaidAMD);
                    }
                }
            }

            return agencyDebtsPreparedData;
        }

        public async Task<List<ReportSalesByManager>?> GetSalesByManagersAsync<T>(DateTime? orderDateFrom = null, DateTime? orderDateTo = null, enumBookStatusForMaratuk bookStatus = enumBookStatusForMaratuk.All) where T : class
        {
            List<ReportSalesByManager> retValue = new();
            ReportSalesByManager newInfo = new();
            var salesByManagersPreparedData = await _reportRepository.GetSalesByManagersPreparedDataAsync<T>(orderDateFrom, orderDateTo);

            if (salesByManagersPreparedData != null)
            {
                foreach (var item in salesByManagersPreparedData as List<ReportSalesByManagerPreparedData>)
                {
                    newInfo = new()
                    {
                        Date = item.Date,
                        OrderNumber = item.OrderNumber,
                        BookStatus = item.BookStatus,
                        AgencyName = item.AgencyName,
                        PassengerName = item.PassengerName + (item.PassengerSurName == null ? "" : " " + item.PassengerSurName),
                        //CostPerTicketInCurrency = Math.Round((double)(item.TicketsCostTotal / item.PassengersCount), 2),
                        CostPerTicketInCurrency = Math.Round((double)item.TicketsCostTotal, 2),
                        HotelName = item.HotelName,
                        TicketsCostTotal = item.TicketsCostTotal,
                        Rate = item.Rate,
                        TicketsCostInAMD = item.TicketsCostInAMD,
                        HotelCostInAMD = item.HotelCostInAMD,
                        Dates = item.TourStartDate?.ToString("dd.MM.yyyy") + (item.TourEndDate == null ? "" : " - " + item.TourEndDate?.ToString("dd.MM.yyyy")),
                        Direction = item.Direction1 + (item.Direction2 == null ? "" : ", " + item.Direction2),
                        FlightManagerName = item.FlightManagerName,
                        HotelManagerName = item.HotelManagerName,
                        TicketsCount = item.TicketsCount,
                        TotalInAMD = item.TicketsCostInAMD + item.HotelCostInAMD
                    };
                    retValue.Add(newInfo);
                }
            }

            return retValue;
            //return salesByManagersPreparedData;



        }

        public async Task<List<ReportTotal>?> GetReportTotalAsync<T>(DateTime? orderDateFrom = null, DateTime? orderDateTo = null, enumBookStatusForMaratuk bookStatus = enumBookStatusForMaratuk.All) where T : class
        {
            List<ReportTotal> retValue = new();
            ReportTotal newInfo = new();
            var salesByManagersPreparedData = await _reportRepository.GetReportTotalPreparedDataAsync<T>(orderDateFrom, orderDateTo);

            if (salesByManagersPreparedData != null && salesByManagersPreparedData.Count > 0)
            {
                foreach (var item in salesByManagersPreparedData as List<ReportTotalPreparedData>)
                {
                    newInfo = new()
                    {
                        DateOfOrder = item.DateOfOrder,
                        OrderNumber = item.OrderNumber,
                        BookStatus = item.BookStatus,
                        CompanyName = item.CompanyName,
                        PassengerName = item.PassengerName + (item.PassengerSurName == null ? "" : " " + item.PassengerSurName),
                        RoomPrice = (item.AccomodationDaysCount == 0) ? Math.Round((double)item.HotelTotal, 2) : Math.Round((double)(item.HotelTotal / item.AccomodationDaysCount), 2),
                        AccomodationDaysCount = item.AccomodationDaysCount,
                        HotelTotal = item.HotelTotal,
                        Total = item.HotelTotal,
                        FlightStartDate = item.FlightStartDate,
                        DepartureTime = item.DepartureTime.HasValue ? item.DepartureTime.Value.ToString("HH:mm") : string.Empty,
                        FlightEndDate = item.FlightEndDate,
                        ArrivalTime = item.ArrivalTime.HasValue ? item.ArrivalTime.Value.ToString("HH:mm") : string.Empty,
                        TourManager = item.TourManager,
                        Rate = item.Rate,
                        PaymentMethod = string.Empty,
                        PaidUnpaid = string.Empty,
                        Confirm = string.Empty,
                        TicketsCount = item.TicketsCount
                    };
                    retValue.Add(newInfo);
                }
            }

            return retValue;
            //return salesByManagersPreparedData;



        }
    }
}
