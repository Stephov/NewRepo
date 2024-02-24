using AutoMapper;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Utils;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class BookedHotelManager : IBookedHotelManager
    {
        private readonly IBookedHotelRepository _bookedHotelRepository;
        //private readonly ICountryManager _countryManager;
        //private readonly IFlightRepository _flightRepository;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public BookedHotelManager(IMapper mapper,
                                    IBookedHotelRepository bookedHotelRepository,
                                    //ICountryManager countryManager,
                                    ICurrencyRatesRepository currencyRatesRepository,
                                    IUserRepository userRepository
                                    //IFlightRepository flightRepository
                                    )
        {

            _bookedHotelRepository = bookedHotelRepository;
            //_countryManager = countryManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _currencyRatesRepository = currencyRatesRepository;
            //_flightRepository = flightRepository;
        }
        public async Task<bool> AddBookedHotelAsync(AddBookHotelRequest addBookedHotel)
        {
            try
            {
                var USDRate = _currencyRatesRepository.GetAsync(1).Result.OfficialRate;
                string agentName = string.Empty;
                string companyName = string.Empty;
                string agentPhone = string.Empty;
                string agentEmail = string.Empty;
                List<string> listOfGuests = new List<string>();
                //string Fligthname1 = string.Empty;
                //string FligthNumber1 = string.Empty;
                //string Fligthname2 = string.Empty;
                //string FligthNumber2 = string.Empty;
                string TotalCurrency = string.Empty;
                string totalPay = string.Empty;
                string maratukAgentEmail = string.Empty;
                List<BookedHotelGuest> bookedHotelGuests = new();


                string orderNumber = "HOT" + RandomNumberGenerators.GenerateRandomNumber(10);

                BookedHotel bookedHotel = new()
                {
                    OrderNumber = orderNumber,
                    //AgentId = addBookedHotel.AgentId,
                    CountryId = addBookedHotel.CountryId,
                    //DateOfOrder = DateTime.Now,
                    ToureTypeId = "Hotel",
                    //HotelId = 1,
                    HotelCode = addBookedHotel.HotelCode,
                    TotalPrice = addBookedHotel.TotalPrice,
                    //Rate = addBookedHotel.Rate,
                    TotalPriceAmd = USDRate * addBookedHotel.TotalPrice,
                    GuestsCount = addBookedHotel.GuestsCount,
                    AccomodationStartDate = addBookedHotel.TourStartDate,
                    AccomodationEndDate = addBookedHotel.TourEndDate,
                    //MaratukAgentId = addBookedHotel.MaratukAgentId,
                    Dept = addBookedHotel.TotalPrice
                };

                foreach (var hotelGuest in addBookedHotel.Guests)
                {
                    BookedHotelGuest bookedGuest = new()
                    {
                        OrderNumber = orderNumber,
                        IsAdult = hotelGuest.IsAdult,
                        GenderId = hotelGuest.GenderId,
                        Name = hotelGuest.Name,
                        Surname = hotelGuest.SurName,
                        PhoneNumber = hotelGuest.PhoneNumber,
                        BirthDay = hotelGuest.BirthDate,
                        Email = hotelGuest.Email,
                        Passport = hotelGuest.Passport,
                        PassportExpiryDate = hotelGuest.PasportExpiryDate
                    };

                    bookedHotelGuests.Add(bookedGuest);

                    //var fligth = await _flightRepository.GetFlightByIdAsync(booked.StartFlightId);
                    //Fligthname1 = fligth.Name;
                    //FligthNumber1 = fligth.FlightValue;
                    //totalPay = booked.TotalPrice.ToString();

                    //if (booked.EndFlightId != null)
                    //{
                    //    var fligth2 = await _flightRepository.GetFlightByIdAsync(booked.EndFlightId);
                    //    Fligthname2 = fligth.Name;
                    //    FligthNumber2 = fligth.FlightValue;
                    //}

                    listOfGuests.Add(hotelGuest.Name + " " + hotelGuest.SurName);
                }

                await _bookedHotelRepository.CreateBookedHotelAsync(bookedHotel);

                var agent = await _userRepository.GetAgencyUsersByIdAsync(addBookedHotel.AgentId);

                if (agent != null)
                {
                    agentName = agent.FullName;
                    agentEmail = agent.Email;
                    agentPhone = agent.PhoneNumber1;
                }

                var maratukAgent = await _userRepository.GetUserByIdAsync(addBookedHotel.MaratukAgentId);
                if (maratukAgent != null)
                {
                    maratukAgentEmail = maratukAgent.Email;
                }

                string listOfGuestsString = string.Join(", ", listOfGuests);
                string date = DateTime.Now.ToString();
                string textBody = $@"
                                    {orderNumber}
                                    Agent: {companyName} 
                                    Creator: {agentName}
                                    Phone Number: {agentPhone}
                                    Email: {agentEmail}
                                    Full list of guests: {listOfGuestsString}
                                    Total payable: {totalPay} 
                                    Date of sale: {date}";
                //{Fligthname1} / {FligthNumber1} / 08:15-09:15
                //{Fligthname2} / {FligthNumber2} / 22:15-23:15

                MailService.SendEmail(maratukAgentEmail, $"New Request {orderNumber}", textBody);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
