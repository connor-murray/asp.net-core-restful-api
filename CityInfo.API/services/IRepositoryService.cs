using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.services
{
    public interface IRepositoryService
    {
        IEnumerable<CityEntity> GetCities();
        bool CityExists(int cityId);
        CityEntity GetCity(int cityId);
        IEnumerable<PointOfInterestEntity> GetPointsOfInterestForCity(int cityId);
        PointOfInterestEntity GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        void AddPointOfInterestForCity(int cityId, PointOfInterestEntity pointOfInterest);
        bool Save();
        void DeletePointOfInterest(PointOfInterestEntity pointOfInterest);
        CityEntity GetCityWithPointsOfInterest(int cityId);
    }
}
