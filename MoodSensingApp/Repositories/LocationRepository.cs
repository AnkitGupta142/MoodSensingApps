using Microsoft.EntityFrameworkCore;
using MoodSensingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(DbContext dbContext) : base(dbContext)
        {

        }
        public async Task<Location> GetByCoordinates(double latitude, double longitude)
        {
            return await _dbSet.FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);
        }
    }
}
