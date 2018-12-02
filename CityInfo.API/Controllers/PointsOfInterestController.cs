using AutoMapper;
using CityInfo.API.Attributes;
using CityInfo.API.Contracts;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly IRepositoryService _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, IRepositoryService cityInfoRepository)
        {
            _mailService = mailService;
            _logger = logger;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogDebug($"city with id {cityId} could not be found");
                return NotFound();
            }

            var pointsOfInterestEntities = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

            var pointsOfInterest = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestEntities);

            return Ok(pointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{poiId}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int poiId)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogDebug($"city with id {cityId} could not be found");
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, poiId);

            if (pointOfInterestEntity == null)
            {
                _logger.LogDebug($"POI with id {poiId} could not be found");
                return NotFound();
            }

            var pointsOfInterest = Mapper.Map<PointOfInterestDto>(pointOfInterestEntity);

            return Ok(pointsOfInterest);
        }

        [ValidateModel]
        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterest pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                _logger.LogDebug("POI is not defined");
                return BadRequest();
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogDebug($"city with id {cityId} could not be found");
                return NotFound();
            }

            var newPointOfInterestEntity = Mapper.Map<PointOfInterestEntity>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, newPointOfInterestEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "An error occurred attempting to create POI");
            }

            var createdPointOfInterest = Mapper.Map<PointOfInterestDto>(newPointOfInterestEntity);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, poiId = createdPointOfInterest.Id }, createdPointOfInterest);
        }

        [ValidateModel]
        [HttpPut("{cityId}/pointsofinterest/{poiId}")]
        public IActionResult UpdatePointOfInterest(int cityId, int poiId, [FromBody] PointOfInterest pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                _logger.LogDebug("POI is not defined");
                return BadRequest("POI is not defined");
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogDebug($"city with id {cityId} could not be found");
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, poiId);

            if (pointOfInterestEntity == null)
            {
                _logger.LogDebug($"POI with id {poiId} could not be found");
                return NotFound();
            }

            Mapper.Map(pointOfInterest, pointOfInterestEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "An error occurred attempting to update POI");
            }

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{poiId}")]
        public IActionResult PartiallyUpdatePointsOfInterest(int cityId, int poiId, [FromBody] JsonPatchDocument<PointOfInterest> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogDebug("POI is not defined");
                return BadRequest();
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogDebug($"city with id {cityId} could not be found");
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, poiId);

            if (pointOfInterestEntity == null)
            {
                _logger.LogDebug($"POI with id {poiId} could not be found");
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterest>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "An error occurred attempting to update POI");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{poiId}")]
        public IActionResult DeletePointsOfInterest(int cityId, int poiId)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogDebug($"city with id {cityId} could not be found");
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, poiId);

            if (pointOfInterestEntity == null)
            {
                _logger.LogDebug($"POI with id {poiId} could not be found");
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "An error occurred attempting to delete POI");
            }

            _mailService.Send("POI deleted", $"POI ID {pointOfInterestEntity.Id}");

            return NoContent();
        }
    }
}
