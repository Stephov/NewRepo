using MaratukAdmin.Entities.Report;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace MaratukAdmin.Managers.Concrete
{
    public class ReportManager : IReportManager
    {
        private readonly IReportRepository _reportRepository;
        private readonly IContractExportRepository _contractExportRepository;

        public ReportManager(IReportRepository reportRepository,
                            IContractExportRepository contractExportRepository)
        {
            _reportRepository = reportRepository;
            _contractExportRepository = contractExportRepository;
        }



        public async Task<List<FlightReportPreparedData>> GetFlightReportPreparedData()
        {
            return await _reportRepository.GetFlightReportPreparedData();
        }
        public async Task<List<BookUniqueDepartureDatesByFlights>> GetBookUniqueDepartureDates()
        {
            return await _reportRepository.GetBookUniqueDepartureDates();
        }
        public async Task<List<ReportFlightInfo>> GetReportFlightInfo()
        {
            List<ReportFlightInfo> retValue = new();
            DateTime prevDepartureDate = DateTime.MinValue;
            string prevFlightNumber = "NoInfo";
            string prevCurrency = "NoInfo";
            int passengersCountTotal = -1;
            double totalPriceSum = -1;
            bool infoChanged = true;
            ReportFlightInfo newInfo = new();

            List<FlightReportPreparedData> flightReportPreparedData = await GetFlightReportPreparedData();

            if (flightReportPreparedData != null)
            {
                foreach (var pData in flightReportPreparedData)
                {
                    if (prevDepartureDate == DateTime.MinValue || (prevDepartureDate != DateTime.MinValue && prevDepartureDate != pData.DepartureDate)
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
                            DepartureDate = pData.DepartureDate,
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

                    prevDepartureDate = pData.DepartureDate;
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

        public async Task<List<ReportTouristInfo>> GetTouristInfoPreparedData(int priceBlockId)
        {
            var commission = _contractExportRepository.GetFlightCommission(priceBlockId);

            return await _reportRepository.GetTouristInfoPreparedData();
        }
    }
}
