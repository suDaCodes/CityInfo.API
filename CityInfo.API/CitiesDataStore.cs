using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        // It's a static property, this's a singleton patern network
        // 
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto() { Id = 1, Name = "Ho Chi Minh City", Description = "It's a crowded place, but fun!" },
                new CityDto() { Id = 2, Name = "Quy Nhon City", Description = "I love this city so much, it's my hometown." },
                new CityDto() { Id = 3, Name = "Phu Quoc City", Description = "This island is a bright gem!" }
            };
        }
    }
}