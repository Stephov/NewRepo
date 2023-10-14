using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Infrastructure
{
    public class MaratukDbContext : DbContext
    {
        public MaratukDbContext(DbContextOptions<MaratukDbContext> options) : base(options) { }

        public DbSet<User>? Users { get; set; }

        public DbSet<FlightInfoFunction> FlightInfoResults { get; set; }
        public DbSet<SearchResultFunction> SearchResultFunctionOneWay { get; set; }
        public DbSet<SearchResultFunctionTwoWay> SearchResultFunctionTwoWay { get; set; }
        public DbSet<FlightReturnDateForManual> FlightReturnDateForManual { get; set; }
        public DbSet<FlightReturnDate> FlightReturnDate { get; set; }

        public DbSet<AgencyUser>? AgencyUser { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Country> Country { get; set; } = null!;
        public DbSet<AirService> AirService { get; set; } = null!;
        public DbSet<City> City { get; set; } = null!;
        public DbSet<Airport> Airport { get; set; } = null!;
        public DbSet<PricePackage> PricePackage { get; set; } = null!;

        public DbSet<Airline> Airline { get; set; } = null!;
        public DbSet<TripType> TripType { get; set; } = null!;
        public DbSet<PriceBlockState> PriceBlockState { get; set; } = null!;
        public DbSet<Aircraft> Aircraft { get; set; } = null!;

        public DbSet<Flight> Flight { get; set; }

        public DbSet<PriceBlock> PriceBlocks { get; set; }


        public DbSet<PriceBlockServices> PriceBlockServices { get; set; }
        public DbSet<ServicesPricingPolicy> ServicesPricingPolicy { get; set; }

        public DbSet<Schedule> Schedule { get; set; }


        public DbSet<Tarif> Tarif { get; set; } = null!;
        public DbSet<ServiceClass> ServiceClass { get; set; } = null!;
        public DbSet<Season> Season { get; set; } = null!;
        public DbSet<PriceBlockType> PriceBlockType { get; set; } = null!;
        public DbSet<Partner> Partner { get; set; } = null!;
        public DbSet<Currency> Currency { get; set; } = null!;
        public DbSet<CurrencyRates> CurrencyRates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<AgencyUser>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<RefreshToken>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Country>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<PricePackage>()
               .HasKey(r => r.Id);

            modelBuilder.Entity<AirService>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Airport>()
               .HasKey(r => r.Id);

            modelBuilder.Entity<Airline>()
             .HasKey(r => r.Id);

            modelBuilder.Entity<TripType>()
          .HasKey(r => r.Id);


            modelBuilder.Entity<Aircraft>()
          .HasKey(r => r.Id);

            modelBuilder.Entity<Flight>()
         .HasKey(r => r.Id);

            modelBuilder.Entity<PriceBlock>()
         .HasKey(r => r.Id);

            modelBuilder.Entity<Schedule>()
     .HasKey(r => r.Id);

            modelBuilder.Entity<PriceBlockServices>()
     .HasKey(r => r.Id);

            modelBuilder.Entity<ServicesPricingPolicy>()
     .HasKey(r => r.Id);

            modelBuilder.Entity<City>()
               .HasKey(r => r.Id);

            modelBuilder.Entity<PriceBlockState>()
          .HasKey(r => r.Id);
            modelBuilder.Entity<Tarif>()
       .HasKey(r => r.Id);
            modelBuilder.Entity<ServiceClass>()
          .HasKey(r => r.Id);
            modelBuilder.Entity<Season>()
          .HasKey(r => r.Id);
            modelBuilder.Entity<PriceBlockType>()
          .HasKey(r => r.Id);
            modelBuilder.Entity<Partner>()
          .HasKey(r => r.Id);
            modelBuilder.Entity<Currency>()
        .HasKey(r => r.Id);
            modelBuilder.Entity<CurrencyRates>()
        .HasKey(r => r.Id);

            modelBuilder.Entity<FlightInfoFunction>().HasNoKey().ToView("GetFlightInfoByTripType");
            modelBuilder.Entity<SearchResultFunction>().HasNoKey().ToView("GetFlightResultOneWay");
            modelBuilder.Entity<SearchResultFunctionTwoWay>().HasNoKey().ToView("GetFlightResultTwoWay");
            modelBuilder.Entity<FlightReturnDateForManual>().HasNoKey().ToView("GetFlightReturnDateForManual");
            modelBuilder.Entity<FlightReturnDate>().HasNoKey().ToView("GetFlightReturnDate");

        }

    }
}
