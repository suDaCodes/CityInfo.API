using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }


        [HttpGet]
        public ActionResult<IEnumerable<CityDTO>> GetCities() 
        {
            return Ok(_citiesDataStore.Cities);
        }

        
        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            // find city
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

            if (city == null) return NotFound();

            return Ok(city);
        }
    }
}