using MoodSensingApp.Models;
using MoodSensingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Services
{
    public class MoodService : IMoodService
    {
        private readonly IMoodRepository _moodRepository;
        private readonly ILocationRepository _locationRepoitory;

        public MoodService(IMoodRepository moodRepository, ILocationRepository locationRepoitory)
        {
            _moodRepository = moodRepository;
            _locationRepoitory = locationRepoitory;
        }

        public async Task<Dictionary<string, int>> GetMoodFrequencyDistribution(int userId)
        {
            
            var userMoodCaptures = await _moodRepository.GetAsync(mc => mc.UserId == userId);

            // Calculate the mood frequency distribution
            var moodFrequencyDistribution = new Dictionary<string, int>();

             moodFrequencyDistribution = userMoodCaptures
               .GroupBy(mc => mc.Mood)
               .ToDictionary(group => group.Key, group => group.Count());

            return moodFrequencyDistribution;

        }

        public async Task InsertMoodCapture(MoodCapture request)
        {
            if (request.Location != null)
            {
                var existingLocation = await _locationRepoitory.GetByCoordinates(request.Location.Latitude, request.Location.Longitude);
                if (existingLocation != null)
                {

                    request.Location = existingLocation;
                }
                else
                {
                  
                    var newLocation = new Location
                    {
                        Name = request.Location.City,
                        Latitude = request.Location.Latitude,
                        Longitude = request.Location.Longitude,
                        City = request.Location.City
                    };

                    
                   await _locationRepoitory.AddAsync(newLocation);

                   
                    request.LocationId = newLocation.LocationId;
                }
            }

            
            await _moodRepository.AddAsync(request);
        }

        public async Task<List<MoodCapture>> GetMoodCaptures(int userId, string mood)
        {
            var moodCaptures =   await _moodRepository.GetAsync(
        mc => mc.UserId == userId && mc.Mood == mood,
        includeProperties: "Location");
            return moodCaptures;
        }
    }
}
