using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;

namespace MaratukAdmin.Extensions
{
    public static class ManagersConfiguration
    {
        public static IServiceCollection AddManagers(this IServiceCollection services, IConfiguration configuration)
        {
            return services
            .AddScoped<IUserManager, UserManager>()
            .AddScoped<IAdminManager, AdminManager>()
            .AddScoped<IFlightManager, FlightManager>()
            .AddScoped<ICountryManager, CountryManager>()
            .AddScoped<ICityManager, CityManager>()
            .AddScoped<IAirlineManager, AirlineManager>()
            .AddScoped<IAircraftManager, AircraftManager>()
            .AddScoped<IAirServiceManager, AirServiceManager>()
            .AddScoped<IPricePackageManager, PricePackageManager>()
            .AddScoped<IAirportManager, AirportManager>()
            .AddScoped<ITarifManager, TarifManager>()
            .AddScoped<IServiceClassManager, ServiceClassManager>()
            .AddScoped<ISeasonManager, SeasonManager>()
            .AddScoped<IPriceBlockTypeManager, PriceBlockTypeManager>()
            .AddScoped<IPartnerManager, PartnerManager>()
            .AddScoped<ICurrencyManager, CurrencyManager>()
            .AddScoped<ICurrencyRatesManager, CurrencyRatesManager>()
            .AddScoped<IPriceBlockManager, PriceBlockManager>();
        }
    }
}
