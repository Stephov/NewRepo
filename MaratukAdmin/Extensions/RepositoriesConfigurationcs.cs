using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Repositories.Concrete.Sansejour;
using Newtonsoft.Json.Serialization;

namespace MaratukAdmin.Extensions
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IAdminRepository, AdminRepository>()
            .AddScoped<IFlightRepository, FlightRepository>()
            .AddScoped<IHotelRepository, HotelRepository>()
            .AddScoped<IContractExportRepository, ContractExportRepository>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<ICountryRepository, CountryRepository>()
            .AddScoped<ICityRepository, CityRepository>()
            .AddScoped<IAirlineRepository, AirlineRepository>()
            .AddScoped<ITarifRepository, TarifRepository>()
            .AddScoped<IServiceClassRepository, ServiceClassRepository>()
            .AddScoped<ISeasonRepository, SeasonRepository>()
            .AddScoped<IPriceBlockTypeRepository, PriceBlockTypeRepository>()
            .AddScoped<IPriceBlockRepository, PriceBlockRepository>()
            .AddScoped<IPartnerRepository, PartnerRepository>()
            .AddScoped<ICurrencyRepository, CurrencyRepository>()
            .AddScoped<IAircraftRepository, AircraftRepository>()
            .AddScoped<IAirServiceRepository, AirServiceRepository>()
            .AddScoped<IPricePackageRepository, PricePackageRepository>()
            .AddScoped(typeof(IMainRepository<>), typeof(MainRepository<>));
        }
    }
}
