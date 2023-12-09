using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class FunctionRepository : IFunctionRepository
    {
        protected readonly MaratukDbContext _context;

        public FunctionRepository(MaratukDbContext context)
        {
            _context = context;
        }

        public async Task<List<FlightReturnDate>> GetFlightReturnDateAsync(int PriceBlockId, int DepartureCountryId, int DepartureCityId, int DestinationCountryId, int DestinationCityId)
        {
            try
            {
                var results = await _context.FlightReturnDate
           .FromSqlRaw("EXEC GetFlightReturnDate @p0, @p1, @p2, @p3, @p4", PriceBlockId, DepartureCountryId, DepartureCityId, DestinationCountryId, DestinationCityId)
           .ToListAsync();
                return results;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return null;
        }

        public async Task<List<FlightReturnDateForManual>> GetFlightReturnDateForManualAsync(DateTime Date, int FlightId)
        {
            try
            {
                var results = await _context.FlightReturnDateForManual
           .FromSqlRaw("EXEC GetFlightReturnDateForManual @p0, @p1", Date, FlightId)
           .ToListAsync();
                return results;

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return null;
        }

        public async Task<List<FlightInfoFunction>> GetFligthInfoFunctionAsync(int TripTypeId)
        {
            try
            {
                var results = await _context.FlightInfoResults
           .FromSqlRaw("EXEC GetFlightInfoByTripType @p0", TripTypeId)
           .ToListAsync();
                return results;

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return null;
        }


        public async Task<List<SearchResultFunction>> GetFligthOneWayInfoFunctionAsync(int FlightId, DateTime FlightDate)
        {
            try
            {
                var results = await _context.SearchResultFunctionOneWay
           .FromSqlRaw("EXEC GetFlightResultOneWay @p0, @p1", FlightId, FlightDate)
           .ToListAsync();
                return results;

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return null;
        }

        public async Task<List<SearchResultFunctionTwoWay>> GetFligthTwoWayInfoFunctionAsync(int FlightOneWayId, int? FlightReturnedId, DateTime FlightStartDate, DateTime? FlightEndDate)
        {

            var results = await _context.SearchResultFunctionTwoWay
       .FromSqlRaw("EXEC GetFlightResultTwoWay @p0, @p1, @p2, @p3", FlightOneWayId, FlightReturnedId, FlightStartDate, FlightEndDate)
       .ToListAsync();
            return results;


        }


        public async Task<List<FinalFlightSearchResponse>> GetFligthInfoFunctionMockAsync(SearchFlightResult searchFlightResult)
        {
            List<FinalFlightSearchResponse> retValue = GenerateFakeFlights(searchFlightResult);

            return await Task.FromResult(retValue);
        }

        private List<FinalFlightSearchResponse> GenerateFakeFlights(SearchFlightResult searchFlightResult)
        {
            var fakeFlights = new List<FinalFlightSearchResponse>();

            try
            {
                // Generate 10 fake records
                for (int i = 1; i <= 10; i++)
                {
                    double AdultPrice = Faker.RandomNumber.Next(1, 5000);
                    double ChildPrice = Faker.RandomNumber.Next(1, 5000);
                    double InfantPrice = Faker.RandomNumber.Next(1, 5000);
                    double TotalPrice = AdultPrice + ChildPrice + InfantPrice;
                    int durationHours = Faker.RandomNumber.Next(1, 15);

                    var fakeFlight = new FinalFlightSearchResponse
                    {
                        FlightId = Faker.RandomNumber.Next(),
                        CostPerTickets = Faker.RandomNumber.Next(1, 5000),
                        TotalPrice = TotalPrice,
                        NumberOfTravelers = searchFlightResult.Adult + searchFlightResult.Child + searchFlightResult.Infant,
                        DepartureAirportCode = Faker.StringFaker.AlphaNumeric(10),
                        DestinationAirportCode = Faker.StringFaker.AlphaNumeric(10),
                        DepartureTime = Faker.DateTimeFaker.DateTime(),
                        ArrivalTime = Faker.DateTimeFaker.DateTime(),
                        AdultPrice = AdultPrice,
                        ChildPrice = ChildPrice,
                        InfantPrice = InfantPrice,
                        Airline = Faker.Lorem.Words(1).FirstOrDefault(),
                        FlightNumber = Faker.StringFaker.AlphaNumeric(5),
                        DurationHours = durationHours,
                        DurationMinutes = durationHours * 60,
                        CurrencyId = Faker.RandomNumber.Next(1,3)
                    };

                    fakeFlights.Add(fakeFlight);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fakeFlights;
        }
    }
}
