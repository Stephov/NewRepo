using MaratukAdmin.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Services
{
    public static class DbContextService
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MaratukDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("MaratukDb")
                ));
            return services;
        }

    }
}
