using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "MustBeFromHCM")]
    // api/cities/{cityId}/pointsOfInterest
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsOfInterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, 
                                            ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
            {
                return Forbid();
            }
            
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                // log this event
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            var pointOfInterestEntities = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestEntities));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogWarning($"City with id {cityId} wasn't found when accessing GetPointOfInterest.");
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity != null) return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterestEntity));
            
            _logger.LogWarning($"Point of Interest with id {pointOfInterestId} wasn't found when accessing GetPointOfInterest.");
            return NotFound();
        }

        [HttpPost]
        public async  Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreationDTO)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();
            
            // map DTO -> entity
            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterestForCreationDTO);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", 
                                    new { 
                                        cityId = cityId, 
                                        pointOfInterestId = createdPointOfInterestToReturn.Id
                                        },
                                        createdPointOfInterestToReturn);
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, 
                                                            PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null) return NotFound();

            _mapper.Map(pointOfInterestForUpdateDto, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, 
                                                                JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();
            
            // get pointOfInterest entity via id
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null) return NotFound();

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
            
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();
            
            // get pointOfInterest entity via id
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null) return NotFound();
            
            // remove, delete pointOfInterest
            _cityInfoRepository.RemovePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Point of Interest deleted!", 
                        $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted!");
            
            return NoContent();
        }
    }
}