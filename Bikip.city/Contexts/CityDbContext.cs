using CityProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityProject.Contexts
{
    public class CityDbContext : DbContext 
    {
        public DbSet<City> Cities { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public CityDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding data
            modelBuilder.Entity<City>()
                .HasData(
                    new City{Id = 1, CityName = "Hanoi"},
                    new City{Id = 2, CityName = "Dublin"}
                    );
            
            modelBuilder.Entity<Hotel>()
                .HasData(
                    new Hotel{Id = 1, HotelName = "Hilton", CityId = 1, Email = "hilton@gmail.com"},
                    new Hotel{Id = 2, HotelName = "Movenpick", CityId = 2, Email = "movenpick@gmail.com"}
                );
        }

        #region Another way to config DB connection
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     string connString = @"Server=(localdb)\MSSQLLocalDB;Database=CityInfoDB;Trusted_Connection=True;";
        //     optionsBuilder.UseSqlServer(connString);
        // }
        #endregion
        
    }
}