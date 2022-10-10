using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    // api/cities/{cityId}/pointsOfInterest
    [Route("api/cities/{cityId}/pointsOfInterest")]
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
        public async Task<ActionResult<IEnumerable<PointOfInterestDTO>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                // log this event
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            var pointOfInterestEntities = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDTO>>(pointOfInterestEntities));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDTO>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogWarning($"City with id {cityId} wasn't found when accessing GetPointOfInterest.");
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity != null) return Ok(_mapper.Map<PointOfInterestDTO>(pointOfInterestEntity));
            
            _logger.LogWarning($"Point of Interest with id {pointOfInterestId} wasn't found when accessing GetPointOfInterest.");
            return NotFound();
        }

        [HttpPost]
        public async  Task<ActionResult<PointOfInterestDTO>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDTO pointOfInterestForCreationDTO)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();
            
            // map DTO -> entity
            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterestForCreationDTO);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDTO>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", 
                                    new { 
                                        cityId = cityId, 
                                        pointOfInterestId = createdPointOfInterestToReturn.Id
                                        },
                                        createdPointOfInterestToReturn);
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, 
                                                            PointOfInterestForUpdateDTO pointOfInterestForUpdateDto)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null) return NotFound();

            _mapper.Map(pointOfInterestForUpdateDto, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, 
                                                                JsonPatchDocument<PointOfInterestForUpdateDTO> patchDocument)
        {
            return Ok();
            // find city
            // var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            // if (city == null) return NotFound();
            //
            // // get point of interest
            // var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            // if (pointOfInterest == null) return NotFound();
            //
            // var pointOfInterestToPatch = new PointOfInterestForUpdateDTO()
            // {
            //     Name = pointOfInterest.Name,
            //     Description = pointOfInterest.Description
            // };
            //
            // patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            //
            // if (!ModelState.IsValid) return BadRequest(ModelState);
            // if (!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);
            //
            // pointOfInterest.Name = pointOfInterestToPatch.Name;
            // pointOfInterest.Description = pointOfInterestToPatch.Description;
            //
            // return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            return Ok();
            // find city
            // var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            // if (city == null) return NotFound();
            //
            // // get point of interest
            // var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            // if (pointOfInterest == null) return NotFound();
            //
            // city.PointsOfInterest.Remove(pointOfInterest);
            //
            // _mailService.Send("Point of Interest deleted!", $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} was deleted!");
            //
            // return NoContent();
        }
    }
}