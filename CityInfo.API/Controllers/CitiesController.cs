using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDTO>>> GetCities()
        {
            var cityEntities = await _repository.GetCitiesAsync();
            
            // map entities -> dto
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDTO>>(cityEntities));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _repository.GetCityAsync(id, includePointsOfInterest);

            if (city == null) return NotFound();

            return includePointsOfInterest ? 
                Ok(_mapper.Map<CityDTO>(city)) : Ok(_mapper.Map<CityWithoutPointsOfInterestDTO>(city));
        }
    }
}