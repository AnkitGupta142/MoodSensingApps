using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoodSensingApp.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Device.Location;
using MoodSensingApp.RequestModels;
using Microsoft.AspNetCore.Authorization;
using System;

namespace MoodSensingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private ILocationService _locationService;
        private IMoodService _moodService;
        
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Location constructor
        /// </summary>
        /// <param name="locationService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="moodService"></param>
        public LocationsController(ILocationService locationService, IHttpContextAccessor httpContextAccessor, IMoodService moodService)
        {
            _locationService = locationService;
            _httpContextAccessor = httpContextAccessor;
            _moodService = moodService;
        }
        /// <summary>
        /// API endpoint to get the closet location where logged in user is happy
        /// </summary>
        
        /// <returns></returns>
        [Authorize]
        [HttpGet("closest-happy")]
        public async Task<IActionResult> GetClosestHappyLocation()
        {
           
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var userCurrentLocation = await _locationService.GetUserLocation(ipAddress);
            // added  hard coded values as the location service won't work from the same machine, to test this feature we can use ngrok and send the request from diffrent machine
            //to work with ngrok kept the https middleware commented
            userCurrentLocation.Latitude = 28.987509;
            userCurrentLocation.Longitude = 79.414124;

            int userId = 1 ;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null )
            {
                userId = Convert.ToInt32(userIdClaim.Value);
            }
          
            var happyMoodCaptures = await _moodService.GetMoodCaptures(userId, "Joy");
            if (happyMoodCaptures.Count < 1)
            {
                return NoContent();
            }

          
            var locationsWithDistance = happyMoodCaptures.Select(mc =>
            {
                var distance = CalculateDistance(userCurrentLocation.Latitude, userCurrentLocation.Longitude, mc.Location.Latitude, mc.Location.Longitude);
                return new { Location = mc.Location, Distance = distance };
            });

           
            var sortedLocations = locationsWithDistance.OrderBy(ld => ld.Distance);

            
            var closestLocation = sortedLocations.FirstOrDefault()?.Location;
            Location response = new Location();
            response.city = closestLocation.City;
            response.lattitude = closestLocation.Latitude;
            response.longitude = closestLocation.Longitude;

            return Ok(response);
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var sCoord = new GeoCoordinate(lat1, lon1);
            var eCoord = new GeoCoordinate(lat2, lon2);

            return sCoord.GetDistanceTo(eCoord);
        }

    }
}
