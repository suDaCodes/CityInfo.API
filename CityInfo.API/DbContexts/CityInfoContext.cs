using System;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;

        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("Ho Chi Minh City") 
                { 
                    Id = 1,
                    Description = "It's a crowded place, but fun!"
                },
                new City("Quy Nhon City") 
                { 
                    Id = 2, 
                    Description = "I love this city so much, it's my hometown."
                },
                new City("Phu Quoc City") 
                { 
                    Id = 3,
                    Description = "This island is a bright gem!"
                }
            );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Landmark 81")
                    { Id = 1, CityId = 1, Description = "The highest building of the city." },
                new PointOfInterest("Duc Ba Church")
                    { Id = 2, CityId = 1, Description = "The most beautiful church in the city." },
                new PointOfInterest("The Saigon Zoo")
                    { Id = 3, CityId = 1, Description = "One of the oldest zoo in the world." },
                new PointOfInterest("The Quy Nhon Centre Square")
                    { Id = 4, CityId = 2, Description = "Fun, crowded, big." },
                new PointOfInterest("Hon Kho Island")
                    { Id = 5, CityId = 2, Description = "Enjoy sea food, and the sea atmosphere." },
                new PointOfInterest("Ky Co Beach")
                    { Id = 6, CityId = 2, Description = "The beauty of the beach will amaze you." },
                new PointOfInterest("Bai Khem")
                    { Id = 7, CityId = 3, Description = "Relax, enjoy, fresh air, beautiful beach." },
                new PointOfInterest("Vinwonders")
                    { Id = 8, CityId = 3, Description = "It looks like a new place." },
                new PointOfInterest("Bun Quay") 
                    { Id = 9, CityId = 3, Description = "Fresh and delicious." }
            );
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}

