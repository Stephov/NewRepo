using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Infrastructure
{
    public class MaratukDbContext : DbContext
    {
        public MaratukDbContext(DbContextOptions<MaratukDbContext> options) : base(options) { }

        public DbSet<User>? Users { get; set; }

        public DbSet<FlightInfoFunction> FlightInfoResults { get; set; }
        public DbSet<BookedFlight> BookedFlights { get; set; }
        public DbSet<BookedHotel> BookedHotel { get; set; }
        public DbSet<BookedHotelGuest> BookedHotelGuest { get; set; }
        public DbSet<SearchResultFunction> SearchResultFunctionOneWay { get; set; }
        public DbSet<SearchResultFunctionTwoWay> SearchResultFunctionTwoWay { get; set; }
        public DbSet<FlightReturnDateForManual> FlightReturnDateForManual { get; set; }
        public DbSet<RoomSearchResponse> RoomSearchResponse { get; set; }
        public DbSet<RoomSearchResponseLowestPrices> RoomSearchResponseLowestPrices { get; set; }
        public DbSet<FlightReturnDate> FlightReturnDate { get; set; }

        public DbSet<AgencyUser>? AgencyUser { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Country> Country { get; set; } = null!;
        public DbSet<AirService> AirService { get; set; } = null!;
        public DbSet<OrderStatus> OrderStatus { get; set; } = null!;
        public DbSet<City> City { get; set; } = null!;
        public DbSet<Airport> Airport { get; set; } = null!;
        public DbSet<PricePackage> PricePackage { get; set; } = null!;

        public DbSet<Airline> Airline { get; set; } = null!;
        public DbSet<Gender> Gender { get; set; } = null!;
        public DbSet<TripType> TripType { get; set; } = null!;
        public DbSet<PriceBlockState> PriceBlockState { get; set; } = null!;
        public DbSet<Aircraft> Aircraft { get; set; } = null!;

        public DbSet<Flight> Flight { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<RoomType> RoomType { get; set; }
        public DbSet<HotelImage> HotelImage { get; set; }

        public DbSet<SyncSejourContractExportView> SyncSejourContractExportView { get; set; }
        public DbSet<SyncSejourHotel> SyncSejourHotel { get; set; }
        public DbSet<SyncSejourSpoAppOrder> SyncSejourSpoAppOrder { get; set; }
        public DbSet<SyncSejourSpecialOffer> SyncSejourSpecialOffer { get; set; }
        public DbSet<SyncSejourRate> SyncSejourRate { get; set; }
        public DbSet<SyncSejourAccomodationType> SyncSejourAccomodationType { get; set; }
        public DbSet<SyncSejourAccomodationDescription> SyncSejourAccomodationDescription { get; set; }




        public DbSet<SpecialOffer> SpecialOffer { get; set; }
        public DbSet<SpecialOfferRate> SpecialOfferRate { get; set; }
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
        public DbSet<RoomCategory> RoomCategories { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<AgencyUser>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<BookedFlight>()
               .HasKey(e => e.Id);
            
            modelBuilder.Entity<BookedHotel>()
               .HasKey(e => e.Id);
            
            modelBuilder.Entity<BookedHotelGuest>()
               .HasKey(e => e.Id);

            modelBuilder.Entity<RefreshToken>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Country>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<PricePackage>()
               .HasKey(r => r.Id);

            modelBuilder.Entity<AirService>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<OrderStatus>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Airport>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Airline>()
             .HasKey(r => r.Id);

            modelBuilder.Entity<Gender>()
             .HasKey(r => r.Id);

            modelBuilder.Entity<TripType>()
          .HasKey(r => r.Id);

            modelBuilder.Entity<Aircraft>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Flight>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Hotel>()
                .HasKey(r => r.Id);
            
            modelBuilder.Entity<Room>()
                .HasKey(r => r.Id);
            
            modelBuilder.Entity<RoomType>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<HotelImage>()
                .HasKey(r => r.Id);
            
            modelBuilder.Entity<SyncSejourContractExportView>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<SyncSejourHotel>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<SyncSejourSpoAppOrder>()
                    .HasKey(r => r.Id);

            modelBuilder.Entity<SyncSejourSpecialOffer>()
                    .HasKey(r => r.Id);

            modelBuilder.Entity<SyncSejourRate>()
                    .HasKey(r => r.Id);

            modelBuilder.Entity<SyncSejourAccomodationType>()
                    .HasKey(r => r.Id);
            
            modelBuilder.Entity<SyncSejourAccomodationDescription>()
                    .HasKey(r => r.Id);
            
            modelBuilder.Entity<SpecialOffer>()
                    .HasKey(r => r.Id);

            modelBuilder.Entity<SpecialOfferRate>()
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
           
            modelBuilder.Entity<RoomCategory>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<FlightInfoFunction>().HasNoKey().ToView("GetFlightInfoByTripType");
            modelBuilder.Entity<SearchResultFunction>().HasNoKey().ToView("GetFlightResultOneWay");
            modelBuilder.Entity<SearchResultFunctionTwoWay>().HasNoKey().ToView("GetFlightResultTwoWay");
            modelBuilder.Entity<FlightReturnDateForManual>().HasNoKey().ToView("GetFlightReturnDateForManual");
            modelBuilder.Entity<FlightReturnDate>().HasNoKey().ToView("GetFlightReturnDate");
            modelBuilder.Entity<RoomSearchResponse>().HasNoKey().ToView("Sp_Search_Room");
            modelBuilder.Entity<RoomSearchResponseLowestPrices>().HasNoKey().ToView("Sp_Search_Room_LowestPrices");


           
        }

    }
}
