using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Infrastructure
{
    public class MaratukDbContext : DbContext
    {
        public MaratukDbContext(DbContextOptions<MaratukDbContext> options) : base(options) { }

        public DbSet<User>? Users { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Country> Country { get; set; } = null!;
        public DbSet<AirService> AirService { get; set; } = null!;
        public DbSet<City> City { get; set; } = null!;
        public DbSet<Airport> Airport { get; set; } = null!;
        public DbSet<PricePackage> PricePackage { get; set; } = null!;

        public DbSet<Airline> Airline { get; set; } = null!;
        public DbSet<Aircraft> Aircraft { get; set; } = null!;

        public DbSet<Flight> Flight { get; set; }

        public DbSet<Schedule> Schedule { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
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


            modelBuilder.Entity<Aircraft>()
          .HasKey(r => r.Id);

            modelBuilder.Entity<Flight>()
         .HasKey(r => r.Id);

            modelBuilder.Entity<Schedule>()
     .HasKey(r => r.Id);

            modelBuilder.Entity<City>()
               .HasKey(r => r.Id);

        }

    }
}
