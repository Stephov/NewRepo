using MaratukAdmin.Entities.Global;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;

namespace MaratukAdmin.Managers.Concrete
{
    public class CountryManager : ICountryManager
    {
        private readonly IMainRepository<Country> _mainRepository;
        private readonly IMainRepository<City> _cityRepository;
        private readonly IFlightRepository _flightRepository;


        public CountryManager(IMainRepository<Country> mainRepository, IFlightRepository flightRepository, IMainRepository<City> cityRepository)
        {
            _mainRepository = mainRepository;
            _flightRepository = flightRepository;
            _cityRepository = cityRepository;
        }
        public async Task<List<Country>> GetAllCountryesAsync()
        {
            return await _mainRepository.GetAllAsync();
        }

        public async Task<Country> GetCountryNameByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<List<FlightCountryResponse>> GetDistinctCountriesAndCities()
        {

            var res = await _flightRepository.GetAllCountryFlightsAsync();
            List<FlightCountryResponse> result = new List<FlightCountryResponse>();



            foreach (var flight in res)
            {
                List<FlightCity> cityes = new List<FlightCity>();

                FlightCountryResponse flightCountryResponse = new FlightCountryResponse();
                flightCountryResponse.DepartureCountryId = flight.DepartureCountryId;
                flightCountryResponse.CountryName =  GetCountryNameByIdAsync(flight.DepartureCountryId).Result.Name;

                foreach(var key in flight.DepartureCityIds) {
                    FlightCity city = new FlightCity();
                    city.DepartureCityId = key;
                    city.DepartureCityName = _cityRepository.GetAsync(key).Result.Name;

                    cityes.Add(city);
                }

                flightCountryResponse.DepartureCity = cityes;
                result.Add(flightCountryResponse);
            }







            return result;


        }
    }
}
