using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly CityInfoContext _cityInfoContext;

        public RepositoryService(CityInfoContext cityInfoContext)
        {
            _cityInfoContext = cityInfoContext;
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterestEntity pointOfInterest)
        {
            var city = GetCity(cityId);

            city.PointsOfInterest.Add(pointOfInterest);
        }

        public bool CityExists(int cityId)
        {
            return _cityInfoContext.Cities.Any(c => c.Id == cityId);
        }

        public void DeletePointOfInterest(PointOfInterestEntity pointOfInterest)
        {
            _cityInfoContext.PointOfInterests.Remove(pointOfInterest);
        }

        public IEnumerable<CityEntity> GetCities()
        {
            return _cityInfoContext.Cities.OrderBy(city => city.Name).ToList();
        }
        
        public CityEntity GetCity(int cityId)
        {
            return _cityInfoContext.Cities.SingleOrDefault(c => c.Id == cityId);
        }

        public CityEntity GetCityWithPointsOfInterest(int cityId)
        {
            return _cityInfoContext.Cities.Where(c => c.Id == cityId).Include(c => c.PointsOfInterest).FirstOrDefault();
        }

        public PointOfInterestEntity GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _cityInfoContext.PointOfInterests.SingleOrDefault(p => p.CityId == cityId && p.Id == pointOfInterestId);
        }

        public IEnumerable<PointOfInterestEntity> GetPointsOfInterestForCity(int cityId)
        {
            return _cityInfoContext.PointOfInterests.Where(p => p.CityId == cityId).ToList();
        }

        public bool Save()
        {
            return (_cityInfoContext.SaveChanges() >= 0);
        }
    }
}
