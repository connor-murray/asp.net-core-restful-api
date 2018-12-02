using CityInfo.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API
{
    public static class EntityContextSeed
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<CityEntity>()
            {
                new CityEntity()
                {
                    Name = "New York City",
                    Description = "blah",
                    PointsOfInterest = new List<PointOfInterestEntity>()
                    {
                       new PointOfInterestEntity()
                       {
                           Name = "Central Perk",
                           Description = "blah",
                       },
                        new PointOfInterestEntity()
                       {
                           Name = "Trump Towers",
                           Description = "blah",
                       }
                    }
                },
                new CityEntity()
                {
                    Name = "Antwerp",
                    Description = "blah",
                    PointsOfInterest = new List<PointOfInterestEntity>()
                    {
                       new PointOfInterestEntity()
                       {
                           Name = "Half a cathedral",
                           Description = "blah",
                       },
                        new PointOfInterestEntity()
                       {
                           Name = "The Corner Shop",
                           Description = "blah",
                       }
                    }
                },
                new CityEntity()
                {
                    Name = "Paris",
                    Description = "blah",
                    PointsOfInterest = new List<PointOfInterestEntity>()
                    {
                       new PointOfInterestEntity()
                       {
                           Name = "Big Spire",
                           Description = "blah",
                       },
                        new PointOfInterestEntity()
                       {
                           Name = "Smelly river",
                           Description = "blah",
                       }
                    }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
