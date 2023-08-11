using MoodSensingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Repositories
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<Location> GetByCoordinates(double latitude, double longitude);
    }
}
