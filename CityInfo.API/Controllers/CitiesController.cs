using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDTO>> GetCities() 
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        
        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            // find city
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if (city == null) return NotFound();

            return Ok(city);
        }
    }
}