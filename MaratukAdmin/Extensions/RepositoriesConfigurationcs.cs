﻿using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;

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
            .AddScoped<ICountryRepository, CountryRepository>()
            .AddScoped<ICityRepository, CityRepository>()
            .AddScoped<ITripTypeRepository, TripTypeRepository>()
            .AddScoped<IPriceBlockStateRepository, PriceBlockStateRepository>()
            .AddScoped<IAirlineRepository, AirlineRepository>()
            .AddScoped<ITarifRepository, TarifRepository>()
            .AddScoped<IBookedFlightRepository, BookedFlightRepository>()
            .AddScoped<IServiceClassRepository, ServiceClassRepository>()
            .AddScoped<ISeasonRepository, SeasonRepository>()
            .AddScoped<IPriceBlockTypeRepository, PriceBlockTypeRepository>()
            .AddScoped<IPriceBlockRepository, PriceBlockRepository>()
            .AddScoped<IPartnerRepository, PartnerRepository>()
            .AddScoped<ICurrencyRepository, CurrencyRepository>()
            .AddScoped<ICurrencyRatesRepository, CurrencyRatesRepository>()
            .AddScoped<IAircraftRepository, AircraftRepository>()
            .AddScoped<IAirServiceRepository, AirServiceRepository>()
            .AddScoped<IPricePackageRepository, PricePackageRepository>()
            .AddScoped<IFunctionRepository, FunctionRepository>()
            .AddScoped(typeof(IMainRepository<>), typeof(MainRepository<>));
        }
    }
}
