using AutoMapper;
using CityInfo.API.Contracts;
using CityInfo.API.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController: Controller
    {
        private readonly IRepositoryService _cityInfoRepository;
        private readonly ILogger<PointsOfInterestController> _logger;

        public CitiesController(IRepositoryService cityInfoRepository, ILogger<PointsOfInterestController> logger)
        {
            _cityInfoRepository = cityInfoRepository;
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cityEntities = _cityInfoRepository.GetCities();

            var cities = Mapper.Map<IEnumerable<CityDto>>(cityEntities);

            return Ok(cities);
        }

        [HttpGet("{cityId}")]
        public IActionResult GetCity(int cityId)
        {
            var cityEntity = _cityInfoRepository.GetCityWithPointsOfInterest(cityId);

            if (cityEntity == null)
            {
                _logger.LogDebug($"city with id {cityId} could not be found");
                return NotFound();
            }

            var cities = Mapper.Map<CityWithPointOfInterestDto>(cityEntity);

            return Ok(cities);
        }
    }
}
