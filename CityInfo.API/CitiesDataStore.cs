using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDTO> Cities { get; set; }

        // It's a static property, this's a singleton patern network
        // 
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDTO>()
            {
                new CityDTO() 
                { 
                    Id = 1, 
                    Name = "Ho Chi Minh City", 
                    Description = "It's a crowded place, but fun!",
                    PointsOfInterest = new List<PointOfInterestDTO>() {
                        new PointOfInterestDTO() { Id = 1, Name = "Landmark 81", Description = "The highest building of the city." },
                        new PointOfInterestDTO() { Id = 2, Name = "Duc Ba gothedame", Description = "The most beautiful church in the city." },
                        new PointOfInterestDTO() { Id = 3, Name = "The zoo", Description = "One of the oldest zoo in the world." }
                    }
                },
                new CityDTO() 
                { 
                    Id = 2, 
                    Name = "Quy Nhon City", 
                    Description = "I love this city so much, it's my hometown.",
                    PointsOfInterest = new List<PointOfInterestDTO>() {
                        new PointOfInterestDTO() { Id = 4, Name = "The centre square", Description = "Fun, crowded, big." },
                        new PointOfInterestDTO() { Id = 5, Name = "Hon Kho island", Description = "Enjoy sea food, and the sea atmosphere." }
                    }
                },
                new CityDTO() 
                { 
                    Id = 3, 
                    Name = "Phu Quoc City", 
                    Description = "This island is a bright gem!",
                    PointsOfInterest = new List<PointOfInterestDTO>() {
                        new PointOfInterestDTO() { Id = 6, Name = "Bai Khem", Description = "Relax, enjoy, fresh air, beautiful beach." },
                        new PointOfInterestDTO() { Id = 7, Name = "Vinwonders", Description = "It looks like a new place." },
                        new PointOfInterestDTO() { Id = 8, Name = "Bun quay", Description = "Good food." }
                    }
                }
            };
        }
    }
}