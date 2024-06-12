using Bogus;
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
        private Faker faker = new();
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

        public async Task<List<FlightInfoFunction>> GetFligthInfoFunctionAsync(int TripTypeId,bool isOnlyFligth)
        {
            try
            {
                var results = await _context.FlightInfoResults
                           .FromSqlRaw("EXEC GetFlightInfoByTripType @p0, @p1", TripTypeId, isOnlyFligth)
                           .ToListAsync();
                
                //results.ForEach(r => r.Price = Math.Ceiling((double)r.Price));


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
                    double AdultPrice = faker.Random.Number(1, 5000);
                    double ChildPrice = faker.Random.Number(1, 5000);
                    double InfantPrice = faker.Random.Number(1, 5000);
                    double TotalPrice = AdultPrice + ChildPrice + InfantPrice;
                    int durationHours = faker.Random.Number(1, 15);

                    var fakeFlight = new FinalFlightSearchResponse
                    {
                        FlightId = faker.Random.Number(),
                        CostPerTickets = faker.Random.Number(1, 5000),
                        TotalPrice = TotalPrice,
                        NumberOfTravelers = searchFlightResult.Adult + searchFlightResult.Child + searchFlightResult.Infant,
                        DepartureAirportCode = faker.Random.AlphaNumeric(10),
                        DestinationAirportCode = faker.Random.AlphaNumeric(10),
                        DepartureTime = faker.Date.Recent(),
                        ArrivalTime = faker.Date.Recent(),
                        AdultPrice = AdultPrice,
                        ChildPrice = ChildPrice,
                        InfantPrice = InfantPrice,
                        Airline = faker.Lorem.Word(),
                        FlightNumber = faker.Random.AlphaNumeric(5),
                        DurationHours = durationHours,
                        DurationMinutes = durationHours * 60,
                        CurrencyId = faker.Random.Number(1, 3)
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
