using AutoMapper;
using Bogus.DataSets;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Numerics;
using static MaratukAdmin.Utils.Enums;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class BookedFlightAndHotelManager : IBookedFlightAndHotelManager
    {
        protected readonly MaratukDbContext _dbContext;
        private readonly IBookedFlightRepository _bookedFlightRepository;
        private readonly IBookedHotelRepository _bookedHotelRepository;
        private readonly IBookedFlightAndHotelRepository _bookedFlightAndHotelRepository;

        private readonly IHotelRepository _hotelRepository;
        //private readonly IContractExportRepository _contractExportRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICountryManager _countryManager;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IAirportManager _airportManager;
        private readonly IMapper _mapper;

        public BookedFlightAndHotelManager(MaratukDbContext dbContext,
                                            IBookedFlightRepository bookedFlightRepository,
                                            IBookedHotelRepository bookedHotelRepository,
                                            IBookedFlightAndHotelRepository bookedFlightAndHotelRepository,
                                            IHotelRepository hotelRepository,
                                            //IContractExportRepository contractExportRepository,
                                            ITransactionRepository transactionRepository,
                                            ICountryManager countryManager,
                                            IUserRepository userRepository,
                                            ICurrencyRatesRepository currencyRatesRepository,
                                            IFlightRepository flightRepository,
                                            IAirportManager airportManager,
                                            IMapper mapper
                                            )
        {
            _dbContext = dbContext;
            _bookedFlightRepository = bookedFlightRepository;
            _bookedHotelRepository = bookedHotelRepository;
            _bookedFlightAndHotelRepository = bookedFlightAndHotelRepository;
            _hotelRepository = hotelRepository;
            //_contractExportRepository = contractExportRepository;
            _transactionRepository = transactionRepository;
            _countryManager = countryManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _currencyRatesRepository = currencyRatesRepository;
            _flightRepository = flightRepository;
            _airportManager = airportManager;
        }

        public async Task<string> AddBookedFlightAndHotelAsync(BookedFlightAndHotel bookedFlightAndHotel)
        {
            string orderNumber = "";
            try
            {
                //var bookedFlightOrderNumber = await _bookedFlightManager.AddBookedFlightAsync(addBookedFlights);

                var USDRate = _currencyRatesRepository.GetAsync(1).Result.OfficialRate;
                string agentName = string.Empty;
                string agentNameHotel = string.Empty;
                string companyName = string.Empty;
                string agentPhone = string.Empty;
                string agentPhoneHotel = string.Empty;
                string agentEmail = string.Empty;
                string agentEmailHotel = string.Empty;
                List<string> listOfArrivals = new List<string>();
                List<string> listOfGuests = new List<string>();
                string Fligthname1 = string.Empty;
                string FligthNumber1 = string.Empty;
                string Fligthname2 = string.Empty;
                string FligthNumber2 = string.Empty;
                string TotalCurrency = string.Empty;
                string totalPay = string.Empty;
                string totalPayHotel = string.Empty;
                double totalPayFlight = 0;
                string maratukAgentEmail = string.Empty;
                string maratukAgentEmailHotel = string.Empty;
                //List<BookedHotelGuest> bookedHotelGuests = new();
                int guestsCount = 0;

                //DateTime tourStartDate = DateTime.MinValue;
                //DateTime? tourEndDate = null;
                int countryId = 0;
                string? hotelName = string.Empty;
                string? hotelCountry = string.Empty;
                string? hotelCity = string.Empty;
                DateTime accomodationDateBegin = DateTime.MinValue;
                DateTime accomodationDateEnd = DateTime.MinValue;
                string strLateCheckout = "No";
                int accomodationDaysCount = 1;
                int hotelAgentId = 0;
                int maratukHotelAgentId = 0;

                // *** Flight part
                int countFligth = await _bookedFlightRepository.GetBookedFlightCountAsync();

                Flight fligthRes = null;

                List<AddBookedFlight> addBookedFlight = bookedFlightAndHotel.BookedFlights;
                if (addBookedFlight.First().EndFlightId == null)
                {
                    fligthRes = await _flightRepository.GetFlightByIdAsync(addBookedFlight.First().StartFlightId);
                }
                else
                {
                    fligthRes = await _flightRepository.GetFlightByIdAsync(addBookedFlight.First().EndFlightId);
                }

                string AirCode = _airportManager.GetAirportNameByIdAsync(fligthRes.DepartureAirportId).Result.Code;

                var schedule1 = await _flightRepository.GetFlightSchedulesByIdAsync(addBookedFlight.First().StartFlightId);

                Flight schedule2 = null;

                if (addBookedFlight.First().EndFlightId != null)
                {
                    schedule2 = await _flightRepository.GetFlightSchedulesByIdAsync((int)addBookedFlight.First().EndFlightId);
                }

                countFligth++;
                orderNumber = AirCode + countFligth.ToString("D8");

                //await _transactionRepository.BeginTransAsync();                                             // Begin transaction
                var strategy = _transactionRepository.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    await _transactionRepository.BeginTransAsync();                                             // Begin transaction
                    {

                        foreach (var bookedFlight in addBookedFlight)
                        {
                            BookedFlight booked = new BookedFlight();
                            booked.OrderNumber = orderNumber;
                            booked.Name = bookedFlight.Name;
                            booked.Surname = bookedFlight.Surname;
                            booked.PhoneNumber = bookedFlight.PhoneNumber;
                            booked.BirthDay = bookedFlight.BirthDay;
                            booked.Email = bookedFlight.Email;
                            booked.Passport = bookedFlight.Passport;
                            booked.AgentId = bookedFlight.AgentId;
                            booked.DateOfOrder = DateTime.Now;
                            booked.ToureTypeId = "Flight + Hotel";
                            booked.TotalPrice = bookedFlight.TotalPrice + bookedFlightAndHotel.HotelTotalPrice;
                            booked.Rate = bookedFlight.Rate;
                            booked.TotalPriceAmd = (USDRate * bookedFlight.TotalPrice) + (USDRate * bookedFlightAndHotel.HotelTotalPrice);
                            booked.PassengersCount = bookedFlight.PassengersCount;
                            booked.TourStartDate = bookedFlight.TourStartDate;
                            booked.TourEndDate = bookedFlight.TourEndDate;
                            //booked.MaratukFlightAgentId = bookedFlight.MaratukFlightAgentId;
                            booked.MaratukFlightAgentId = 22;                                   // Inesa Sevinyan
                            booked.MaratukHotelAgentId = bookedFlight.MaratukHotelAgentId;
                            booked.CountryId = bookedFlight.CountryId;
                            booked.StartFlightId = bookedFlight.StartFlightId;
                            booked.EndFlightId = bookedFlight.EndFlightId;
                            booked.PasportExpiryDate = bookedFlight.PasportExpiryDate;
                            booked.GenderId = bookedFlight.GenderId;
                            booked.PassengerTypeId = bookedFlight.PassengerTypeId;
                            booked.Dept = bookedFlight.TotalPrice + bookedFlightAndHotel.HotelTotalPrice;
                            booked.HotelId = bookedFlightAndHotel.HotelId;
                            booked.BookStatusForClient = (int)Enums.enumBookStatusForClient.Waiting;
                            booked.BookStatusForMaratuk = (int)Enums.enumBookStatusForMaratuk.Waiting;

                            var fligth = await _flightRepository.GetFlightByIdAsync(booked.StartFlightId);
                            Fligthname1 = fligth.Name;
                            FligthNumber1 = fligth.FlightValue;
                            //totalPay = booked.TotalPrice.ToString();
                            totalPayFlight = booked.TotalPrice;

                            if (booked.EndFlightId != null)
                            {
                                var fligth2 = await _flightRepository.GetFlightByIdAsync(booked.EndFlightId);
                                Fligthname2 = fligth2.Name;
                                FligthNumber2 = fligth2.FlightValue;
                            }

                            var agent = await _userRepository.GetAgencyUsersByIdAsync(booked.AgentId);

                            if (agent != null)
                            {
                                companyName = agent.FullCompanyName;
                                agentName = agent.FullName;
                                agentEmail = agent.Email;
                                agentPhone = agent.PhoneNumber1;
                            }

                            listOfArrivals.Add(booked.Name + " " + booked.Surname);
                            var maratukAgent = await _userRepository.GetUserByIdAsync(booked.MaratukFlightAgentId);
                            if (maratukAgent != null)
                            {
                                maratukAgentEmail = maratukAgent.Email;
                            }

                            // Book Flight
                            await _bookedFlightRepository.CreateBookedFlightAsync(booked);

                            // Variables to use for Hotel
                            guestsCount++;
                            countryId = booked.CountryId;
                            hotelAgentId = booked.AgentId;
                            maratukHotelAgentId = (int)booked.MaratukHotelAgentId;

                            listOfGuests.Add(booked.Name + " " + booked.Surname);
                        }

                        // *** Hotel part
                        accomodationDateBegin = bookedFlightAndHotel.AccomodationStartDate;
                        accomodationDateEnd = (DateTime)bookedFlightAndHotel.AccomodationEndDate;
                        accomodationDateEnd.AddDays((bookedFlightAndHotel.LateCheckout) ? 1 : 0);
                        strLateCheckout = (bookedFlightAndHotel.LateCheckout) ? "Yes" : "No";

                        accomodationDaysCount = (int)((DateTime)accomodationDateEnd - accomodationDateBegin).TotalDays;

                        BookedHotel bookedHotel = new()
                        {
                            OrderNumber = orderNumber,
                            Room = bookedFlightAndHotel.Room,
                            RoomCode = bookedFlightAndHotel.RoomType,
                            HotelId = bookedFlightAndHotel.HotelId,
                            HotelCode = bookedFlightAndHotel.HotelCode,
                            SejourRateId = bookedFlightAndHotel.SejourRateId,
                            CountryId = countryId,
                            ToureTypeId = "Hotel",
                            // todo miguce Price toxnel ev rename anel -> PricePerDay ?
                            Price = bookedFlightAndHotel.Price,
                            HotelTotalPrice = bookedFlightAndHotel.HotelTotalPrice,
                            HotelTotalPriceAmd = USDRate * bookedFlightAndHotel.HotelTotalPrice,
                            GuestsCount = guestsCount,
                            AccomodationStartDate = bookedFlightAndHotel.AccomodationStartDate,
                            AccomodationEndDate = bookedFlightAndHotel.AccomodationEndDate,
                            AccomodationDaysCount = accomodationDaysCount,
                            LateCheckout = bookedFlightAndHotel.LateCheckout,
                            Dept = bookedFlightAndHotel.HotelTotalPrice,
                            Board = bookedFlightAndHotel.Board,
                            BoardDesc = bookedFlightAndHotel.BoardDesc,
                            //HotelAgentId = bookedFlightAndHotel.HotelAgentId,
                            HotelAgentId = hotelAgentId,
                            BookStatusForClient = (int)Enums.enumBookStatusForClient.Waiting,
                            //MaratukHotelAgentId = bookedFlightAndHotel.MaratukHotelAgentId,
                            MaratukHotelAgentId = maratukHotelAgentId,
                            BookStatusForMaratuk = (int)Enums.enumBookStatusForClient.Waiting
                        };

                        var agentHotel = await _userRepository.GetAgencyUsersByIdAsync(bookedHotel.HotelAgentId);

                        if (agentHotel != null)
                        {
                            agentNameHotel = agentHotel.FullName;
                            agentEmailHotel = agentHotel.Email;
                            agentPhoneHotel = agentHotel.PhoneNumber1;
                        }

                        var maratukAgentHotel = await _userRepository.GetUserByIdAsync(bookedHotel.MaratukHotelAgentId);
                        if (maratukAgentHotel != null)
                        {
                            maratukAgentEmailHotel = maratukAgentHotel.Email;
                        }

                        // Book Hotel
                        await _bookedHotelRepository.CreateBookedHotelAsync(bookedHotel);

                        var hotel = await _hotelRepository.GetHotelByCodeAsync(bookedFlightAndHotel.HotelCode);

                        if (hotel != null && hotel.hotel != null)
                        {
                            hotelName = hotel.hotel.Name;
                            hotelCountry = hotel.hotelCountryNameEng;
                            hotelCity = hotel.hotelCityNameEng;
                        }

                        //totalPay = (totalPayFlight + bookedHotel.HotelTotalPrice).ToString();
                        totalPay = totalPayFlight.ToString();
                        totalPayHotel = bookedHotel.HotelTotalPrice.ToString();


                        await _transactionRepository.CommitTransAsync();                                            // Commit transaction
                        //await transaction.CommitAsync();
                    }
                });
                string listOfArrivalsString = string.Join(", ", listOfArrivals);
                string listOfGuestsString = string.Join(", ", listOfGuests);
                string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                //string textBody = $@"
                //                    {orderNumber}
                //                    Agent: {companyName} 
                //                    Creator: {agentName}
                //                    Phone Number: {agentPhone}
                //                    Email: {agentEmail}
                //                    Full list of arrivals: {listOfArrivalsString}
                //                    {Fligthname1} / {FligthNumber1} / 08:15-09:15
                //                    {Fligthname2} / {FligthNumber2} / 22:15-23:15
                //                    Hotel: {hotelName}
                //                    City/Country: {hotelCity} / {hotelCountry}
                //                    Country: {hotelCountry}
                //                    Total payable: {totalPay} 
                //                    Date of sale: {date}";
                string textBodyFlight = $@"
<html>
<head>
  <title>Flight Booking Details</title>
</head>
<body>
  <p>{orderNumber}</p>
  <p>Agent: {companyName}</p>
  <p>Creator: {agentName}</p>
  <p>Phone Number: {agentPhone}</p>
  <p>Email: {agentEmail}</p>
  <p>Full list of arrivals: {listOfArrivalsString}</p>
  <p>Departure Date: {addBookedFlight?.First().TourStartDate.ToString("yyyy-MM-dd")}</p>
  <p>Arrival Time: {addBookedFlight?.First().TourEndDate?.ToString("yyyy-MM-dd")}</p>
  <p>{Fligthname1} / {FligthNumber1} / {schedule1.Schedules.First().DepartureTime.TimeOfDay.ToString("hh\\:mm")}-{schedule1.Schedules.First().ArrivalTime.TimeOfDay.ToString("hh\\:mm")}</p>
  {(schedule2 != null ? $"<p>{Fligthname2} / {FligthNumber2} / {schedule2?.Schedules.First().DepartureTime.TimeOfDay.ToString("hh\\:mm")}-{schedule2?.Schedules.First().ArrivalTime.TimeOfDay.ToString("hh\\:mm")}</p>" : "")}
  <p>Total payable: {totalPay}</p>
  <p>Date of sale: {date}</p>
</body>
</html>";




                MailService.SendEmail(maratukAgentEmail, $"New Request {orderNumber}", textBodyFlight);

                string textBodyHotel = $@"
<html>
<head>
  <title>Hotel Booking Details</title>
</head>
<body>
  <p>{orderNumber}</p>
  <p>Agent: {companyName}</p>
  <p>Creator: {agentNameHotel}</p>
  <p>Phone Number: {agentPhoneHotel}</p>
  <p>Email: {agentEmailHotel}</p>
  <p>Departure Date: {addBookedFlight?.First().TourStartDate.ToString("dd.MM.yyyy")}</p>
  <p>Arrival Time: {addBookedFlight?.First().TourEndDate?.ToString("dd.MM.yyyy")}</p>
  <p>{Fligthname1} / {FligthNumber1} / {schedule1.Schedules.First().DepartureTime.TimeOfDay.ToString("hh\\:mm")}-{schedule1.Schedules.First().ArrivalTime.TimeOfDay.ToString("hh\\:mm")}</p>
  {(schedule2 != null ? $"<p>{Fligthname2} / {FligthNumber2} / {schedule2?.Schedules.First().DepartureTime.TimeOfDay.ToString("hh\\:mm")}-{schedule2?.Schedules.First().ArrivalTime.TimeOfDay.ToString("hh\\:mm")}</p>" : "")}
  <p>Hotel: {hotelName}</p>
  <p>Room type: {bookedFlightAndHotel.RoomType}</p>
  <p>Room code: {bookedFlightAndHotel.Room}</p>
  <p>City/Country: {hotelCity} / {hotelCountry}</p>
  <p>Accommodation dates: {accomodationDateBegin.ToString("dd.MM.yyyy")} / {accomodationDateEnd.ToString("dd.MM.yyyy")}</p>
  <p>Late checkout: {strLateCheckout}</p>
  <p>Days count: {accomodationDaysCount}</p>
  <p>List of guests: {listOfGuestsString}</p>
  <p>Total payable: {totalPay}</p>
</body>
</html>";
/*    <p>Date of sale: {date}</p> 
*/

/*
<p>Room type: {(string)bookedFlightAndHotel.RoomType}</p>
<p>Room code: {(string)bookedFlightAndHotel.Room}</p>
*/

                MailService.SendEmail(maratukAgentEmailHotel, $"New Request {orderNumber}", textBodyHotel);
            }
            catch (Exception)
            {
                //await _transactionRepository.RollbackTransAsync();        // Rollback transaction
                throw;
            }

            return orderNumber;
        }


        public async Task<string> PayForBookedFlightAndHotelAsync(PayForBookedFlightAndHotelRequest payForBookedFlightAndHotel)
        {
            string retValue = "OK";
            try
            {
                //var USDRate = _currencyRatesRepository.GetAsync(1).Result.OfficialRate;
                double paidAMD = 0;
                double bookPaidAMD = 0;
                double paidInCurrency = 0;
                double bookPaidInCurrency = 0;
                int paymentSign = (payForBookedFlightAndHotel.PaymentType.ToString() == Enums.enumBookPaymentTypes.D.ToString()) ? 1 : (-1);
                double totalDebt = 0;
                int paymentStatus = 0;
                BookPayment existingBookPayment = new();

                var strategy = _transactionRepository.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    await _transactionRepository.BeginTransAsync();                                             // Begin transaction
                    {

                        if (!Enum.TryParse(typeof(enumBookPaymentStatuses), payForBookedFlightAndHotel.PaymentStatus, true, out var status))
                        {
                            throw new IncorrectDataException("Invalid PaymentStatus value.");
                        }

                        paymentStatus = (int)status;

                        if (paymentStatus != (int)Enums.enumBookPaymentStatuses.InProcess)
                        {
                            // Get BookPayment for given OrderNumber and PaymentNumber
                            existingBookPayment = await _bookedFlightAndHotelRepository.GetBookPaymentAsync(default, payForBookedFlightAndHotel.OrderNumber, payForBookedFlightAndHotel.PaymentNumber);

                            if (existingBookPayment != null)
                            {
                                // Define paid values
                                //bookPaidAMD = existingBookPayment.SumAMD;
                                //bookPaidInCurrency = existingBookPayment.SumInCurrency;
                                bookPaidAMD = (double)payForBookedFlightAndHotel.PaidAMD;
                                bookPaidInCurrency = (double)payForBookedFlightAndHotel.PaidInCurrency;

                                if (paymentStatus == (int)Enums.enumBookPaymentStatuses.Approved)
                                {
                                    existingBookPayment.PaidAMD += bookPaidAMD;
                                    existingBookPayment.PaidInCurrency += bookPaidInCurrency;
                                }
                                // We don't need to update Sums in case of DECLINE or CANCEL the payment
                                //else if (paymentStatus == (int)Enums.enumBookPaymentStatuses.Declined || paymentStatus == (int)Enums.enumBookPaymentStatuses.Cancelled)
                                //{
                                //    existingBookPayment.PaidAMD -= bookPaidAMD;
                                //    existingBookPayment.PaidInCurrency -= bookPaidInCurrency;
                                //}

                                existingBookPayment.PaymentStatus = paymentStatus;

                                if (existingBookPayment.PaidAMD > existingBookPayment.SumAMD || existingBookPayment.PaidInCurrency > existingBookPayment.SumInCurrency)
                                {
                                    throw new IncorrectDataException("You cannot pay more than the initial amount.");
                                }
                            }
                            else
                            {
                                throw new IncorrectDataException("Payment was not found.");
                            }
                        }

                        List<BookedFlight> bookedFlights = await _bookedFlightRepository.GetBookedFlightByOrderNumberAsync(payForBookedFlightAndHotel.OrderNumber);

                        if (bookedFlights != null && bookedFlights.Count > 0)
                        {
                            foreach (var flight in bookedFlights)
                            {
                                //paidAMD = (payForBookedFlightAndHotel.SumAMD == null) ? 0 : (double)payForBookedFlightAndHotel.SumAMD * paymentSign;
                                //paidInCurrency = (payForBookedFlightAndHotel.SumInCurrency == null) ? 0 : (double)payForBookedFlightAndHotel.SumInCurrency * paymentSign;
                                paidAMD = bookPaidAMD * paymentSign;
                                paidInCurrency = bookPaidInCurrency * paymentSign;
                                flight.Paid += paidInCurrency;

                                if (paymentStatus != (int)Enums.enumBookPaymentStatuses.InProcess)
                                {

                                    if (paymentStatus == (int)Enums.enumBookPaymentStatuses.Approved)
                                    {
                                        totalDebt = (double)(flight.Dept - paidInCurrency);
                                    }

                                    if (flight.TotalPrice < totalDebt)
                                    {
                                        throw new IncorrectDataException("In result of this operation the Debt will exceed the Total price.");
                                    }
                                    if (totalDebt < 0)
                                    {
                                        throw new IncorrectDataException("In result of this operation payment(s) will exceed the Total price.");
                                    }
                                    else if (totalDebt == 0)
                                    {
                                        flight.BookStatusForClient = (int)Enums.enumBookStatusForClient.FullyPaid;
                                        flight.BookStatusForMaratuk = (int)Enums.enumBookStatusForMaratuk.PaidInFull;
                                    }
                                    else if (totalDebt > 0)
                                    {
                                        if (paymentStatus == (int)Enums.enumBookPaymentStatuses.Approved)
                                        {
                                            flight.BookStatusForClient = (int)Enums.enumBookStatusForClient.PartiallyPaid;
                                            flight.BookStatusForMaratuk = (int)Enums.enumBookStatusForMaratuk.PaidPartially;
                                        }
                                        else if (paymentStatus == (int)Enums.enumBookPaymentStatuses.Cancelled
                                                || paymentStatus == (int)Enums.enumBookPaymentStatuses.Declined)
                                        {
                                            if (flight.TotalPrice == (double)(flight.Dept))                                         // If no payment was made
                                            {
                                                flight.BookStatusForClient = (int)Enums.enumBookStatusForClient.ConfirmedByAccountant;
                                                flight.BookStatusForMaratuk = (int)Enums.enumBookStatusForMaratuk.ConfirmedByAccountant;
                                            }
                                            else
                                            {
                                                flight.BookStatusForClient = (int)Enums.enumBookStatusForClient.InvoiceSent;
                                                flight.BookStatusForMaratuk = (int)Enums.enumBookStatusForMaratuk.InvoiceSent;
                                            }
                                        }
                                    }
                                }
                                // InProcess
                                else
                                {
                                    flight.BookStatusForClient = (int)Enums.enumBookStatusForClient.InvoiceSent;
                                    flight.BookStatusForMaratuk = (int)Enums.enumBookStatusForMaratuk.InvoiceSent;

                                    totalDebt = (double)(flight.Dept);
                                }

                                flight.Dept = totalDebt;
                            }

                            if (paymentStatus == (int)Enums.enumBookPaymentStatuses.Approved)
                            {
                                // Update debt in BookedFlights table
                                await _bookedFlightRepository.UpdateBookedFlightsAsync(bookedFlights);

                                // Update Status of payment in BookPayments table
                                await _bookedFlightAndHotelRepository.UpdateBookPaymentAsync(existingBookPayment);
                            }
                            else
                            {
                                // Add Payment
                                BookPayment newBookPayment = new()
                                {
                                    OrderNumber = payForBookedFlightAndHotel.OrderNumber,
                                    PaymentNumber = payForBookedFlightAndHotel.PaymentNumber,
                                    SumAMD = (double)payForBookedFlightAndHotel.SumAMD,
                                    Currency = payForBookedFlightAndHotel.Currency,
                                    SumInCurrency = (double)payForBookedFlightAndHotel.SumInCurrency,
                                    PaymentType = payForBookedFlightAndHotel.PaymentType.ToString(),
                                    PaymentStatus = paymentStatus,
                                    PayerId = (int)payForBookedFlightAndHotel.PayerId,
                                    PaymentDate = DateTime.Now
                                };
                                await _hotelRepository.AddBookPaymentAsync(newBookPayment);
                            }
                        }
                        await _transactionRepository.CommitTransAsync();                                            // Commit transaction
                    }
                });
            }
            catch (Exception ex) when (ex is IncorrectDataException)
            {
                retValue = ex.Message;
                throw ex;
            }
            catch (Exception ex)
            {
                //await _transactionRepository.RollbackTransAsync();        // Rollback transaction
                throw;
            }

            return retValue;
        }

        public async Task<List<BookedInfoFlightPartResponse>> GetBookedInfoFlighPartAsync(BookedInfoFlightPartRequest request)
        {
            return await _bookedFlightAndHotelRepository.GetBookedInfoFlighPartAsync(request);
        }

        //public Task<List<BookedInfoFlightPartGroupedResponse>?> GetBookedInfoFlighPartGroupAsync(List<BookedInfoFlightPartResponse> request)
        //public async Task<List<BookedInfoFlightPartGroupedResponse>?> GetBookedInfoFlighPartGroupAsync(List<BookedInfoFlightPartResponse> request)
        public async Task<BookedInfoFlightPartGroupedResponse> GetBookedInfoFlighPartGroupAsync(List<BookedInfoFlightPartResponse> request)
        {
            BookedInfoFlightPartGroupedResponse retValue = new();


            var groupedData = request
                        .GroupBy(item => new { item.TourStartDate, item.MaratukAgentStatus })
                        .Select(group => new GroupedFlightInfo
                        {
                            TourStartDate = group.Key.TourStartDate,
                            MaratukAgentStatus = group.Key.MaratukAgentStatus,
                            StatusCount = group.Count(),
                            Summa = group.Sum(item => item.Summa)
                        })
                        .ToList();

            var groupedFlights = new BookedInfoFlightPartGroupedResponse
            {
                groupedFlightInfo = groupedData,
                SummaTotal = groupedData.Sum(item => item.Summa)
            };

            //var groupedFlights = request.GroupBy(stD => new { stD.TourStartDate, stD.MaratukAgentStatus })
            //.Select(group => new BookedInfoFlightPartGroupedResponse()
            //{
            //GroupedFlight = new List<GroupedFlightInfo>()
            //{
            //    TourStartDate = group.Key.TourStartDate,
            //    Summa = group.Select(s => s.Summa).Sum(),
            //    //MaratukAgentStatus = group.Select(stat => stat.MaratukAgentStatus).FirstOrDefault(),
            //    MaratukAgentStatus = group.Key.MaratukAgentStatus,
            //    StatusCount = group.Select(stCnt => stCnt.MaratukAgentStatus).Count()
            //}
            //}
            //).ToList();

            //var groupedFlights = listBookedFlights
            //.GroupBy(flight => new { flight.OrderNumber, flight.Rate })
            //.Select(group => new
            //{
            //    Currency = group.Key.Rate,
            //    TotalDept = group.Select(flight => flight.Dept ?? 0).Distinct().Sum()
            //});

            retValue = groupedFlights;

            //return retValue;
            //throw new NotImplementedException();
            return await Task.FromResult(retValue);

        }

        public async Task<BookPayment> GetBookPaymentAsync(int? id, string? orderNumber, string? paymentNumber)
        {
            return await _bookedFlightAndHotelRepository.GetBookPaymentAsync(id, orderNumber, paymentNumber);
        }

        public async Task<List<BookPayment>> GetBookPaymentsByOrderNumberAsync(string orderNumber)
        {
            return await _bookedFlightAndHotelRepository.GetBookPaymentsByOrderNumberAsync(orderNumber);
        }
    }
}
