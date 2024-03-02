using AutoMapper;
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
                string companyName = string.Empty;
                string agentPhone = string.Empty;
                string agentEmail = string.Empty;
                List<string> listOfArrivals = new List<string>();
                List<string> listOfGuests = new List<string>();
                string Fligthname1 = string.Empty;
                string FligthNumber1 = string.Empty;
                string Fligthname2 = string.Empty;
                string FligthNumber2 = string.Empty;
                string TotalCurrency = string.Empty;
                string totalPay = string.Empty;
                double totalPayFlight = 0;
                string maratukAgentEmail = string.Empty;
                //List<BookedHotelGuest> bookedHotelGuests = new();
                int guestsCount = 0;

                DateTime tourStartDate = DateTime.MinValue;
                DateTime? tourEndDate = null;
                int countryId = 0;
                string? hotelName = string.Empty;
                string? hotelCountry = string.Empty;
                string? hotelCity = string.Empty;
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
                            booked.Dept = bookedFlight.TotalPrice;
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
                                Fligthname2 = fligth.Name;
                                FligthNumber2 = fligth.FlightValue;
                            }

                            var agent = await _userRepository.GetAgencyUsersByIdAsync(booked.AgentId);

                            if (agent != null)
                            {
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
                            await _bookedFlightRepository.CreateBookedFlightAsync(booked);

                            // Variables to use for Hotel
                            guestsCount++;
                            tourStartDate = booked.TourStartDate;
                            tourEndDate = booked.TourEndDate;
                            countryId = booked.CountryId;
                            hotelAgentId = booked.AgentId;
                            maratukHotelAgentId = booked.MaratukHotelAgentId;

                            listOfGuests.Add(booked.Name + " " + booked.Surname);
                        }

                        // *** Hotel part
                        accomodationDaysCount = (int)((DateTime)tourEndDate - tourStartDate).TotalDays;
                        accomodationDaysCount += (bookedFlightAndHotel.LateCheckout) ? 1 : 0;

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
                            Dept = bookedFlightAndHotel.Price,
                            Board = bookedFlightAndHotel.Board,
                            BoardDesc = bookedFlightAndHotel.BoardDesc,
                            //HotelAgentId = bookedFlightAndHotel.HotelAgentId,
                            HotelAgentId = hotelAgentId,
                            BookStatusForClient = (int)Enums.enumBookStatusForClient.Waiting,
                            //MaratukHotelAgentId = bookedFlightAndHotel.MaratukHotelAgentId,
                            MaratukHotelAgentId = maratukHotelAgentId,
                            BookStatusForMaratuk = (int)Enums.enumBookStatusForClient.Waiting
                        };

                        await _bookedHotelRepository.CreateBookedHotelAsync(bookedHotel);

                        var hotel = await _hotelRepository.GetHotelByCodeAsync(bookedFlightAndHotel.HotelCode);

                        if (hotel != null && hotel.hotel != null)
                        {
                            hotelName = hotel.hotel.Name;
                            hotelCountry = hotel.hotelCountryNameEng;
                            hotelCity = hotel.hotelCityNameEng;
                        }

                        totalPay = (totalPayFlight + bookedHotel.HotelTotalPrice).ToString();


                        await _transactionRepository.CommitTransAsync();                                            // Commit transaction
                                                                                                                    //await transaction.CommitAsync();
                    }
                });
                string listOfArrivalsString = string.Join(", ", listOfArrivals);
                string date = DateTime.Now.ToString();
                string textBody = $@"
                                    {orderNumber}
                                    Agent: {companyName} 
                                    Creator: {agentName}
                                    Phone Number: {agentPhone}
                                    Email: {agentEmail}
                                    Full list of arrivals: {listOfArrivalsString}
                                    {Fligthname1} / {FligthNumber1} / 08:15-09:15
                                    {Fligthname2} / {FligthNumber2} / 22:15-23:15
                                    Hotel: {hotelName}
                                    City/Country: {hotelCity} / {hotelCountry}
                                    Country: {hotelCountry}
                                    Total payable: {totalPay} 
                                    Date of sale: {date}";

                MailService.SendEmail(maratukAgentEmail, $"New Request {orderNumber}", textBody);
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
                double paidInCurrency = 0;
                int paymentSign = (payForBookedFlightAndHotel.PaymentType.ToString() == Enums.enumBookPaymentTypes.D.ToString()) ? 1 : (-1);
                double totalDebt = 0;

                var strategy = _transactionRepository.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    await _transactionRepository.BeginTransAsync();                                             // Begin transaction
                    {
                        List<BookedFlight> bookedFlights = await _bookedFlightRepository.GetBookedFlightByOrderNumberAsync(payForBookedFlightAndHotel.OrderNumber);

                        if (bookedFlights != null && bookedFlights.Count > 0)
                        {
                            foreach (var flight in bookedFlights)
                            {
                                paidAMD = payForBookedFlightAndHotel.SumAMD * paymentSign;
                                paidInCurrency = payForBookedFlightAndHotel.SumInCurrency * paymentSign;
                                flight.Paid += paidInCurrency;

                                totalDebt = (double)(flight.Dept - paidInCurrency);
                                if (flight.TotalPrice < totalDebt)
                                {
                                    throw new IncorrectDataException("In result of this operation the Debt will exceed the Total price.");
                                }
                                flight.Dept = totalDebt;
                            }

                            // Update debt in BookedFlights table
                            await _bookedFlightRepository.UpdateBookedFlightsAsync(bookedFlights);

                            // Add Payment
                            BookPayment payment = new()
                            {
                                OrderNumber = payForBookedFlightAndHotel.OrderNumber,
                                PaymentNumber = payForBookedFlightAndHotel.PaymentNumber,
                                SumAMD = payForBookedFlightAndHotel.SumAMD,
                                Currency = payForBookedFlightAndHotel.Currency,
                                SumInCurrency = payForBookedFlightAndHotel.SumInCurrency,
                                PaymentType = payForBookedFlightAndHotel.PaymentType.ToString(),
                                PaymentStatus = payForBookedFlightAndHotel.PaymentStatus,
                                PayerId = payForBookedFlightAndHotel.PayerId,
                                PaymentDate = DateTime.Now
                            };
                            await _hotelRepository.AddBookPaymentAsync(payment);
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



        public async Task<List<BookedHotelResponse>> GetBookedFlightsAsync(int Itn)
        {
            throw new NotImplementedException();
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
    }
}
