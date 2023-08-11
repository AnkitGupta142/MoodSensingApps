using MoodSensingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Services
{
    public interface ILocationService
    {
        Task<Location> GetUserLocation(string ipAddress);
    }
}
