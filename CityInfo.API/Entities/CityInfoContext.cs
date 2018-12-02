using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<PointOfInterestEntity> PointOfInterests { get; set; }
    }
}
