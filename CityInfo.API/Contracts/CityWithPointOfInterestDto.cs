using System.Collections.Generic;

namespace CityInfo.API.Contracts
{
    public class CityWithPointOfInterestDto : CityDto
    {
        public int NumberOfPointsOfInterest {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
    }
}
