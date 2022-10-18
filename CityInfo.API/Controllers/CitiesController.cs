using System.Text.Json;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    // [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;
        private const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(string? name, string? searchQuery, 
                                                                                                int pageNumber, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize) pageSize = maxCitiesPageSize;
            
            
            
            var (cityEntities, paginationMetadata) = 
                await _repository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            
            // map entities -> dto
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _repository.GetCityAsync(id, includePointsOfInterest);

            if (city == null) return NotFound();

            return includePointsOfInterest ? 
                Ok(_mapper.Map<CityDTO>(city)) : Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}