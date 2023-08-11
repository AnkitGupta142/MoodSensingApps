using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoodSensingApp.Mapper;
using MoodSensingApp.Models;
using MoodSensingApp.RequestModels;
using MoodSensingApp.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodsController : ControllerBase
    {
        private ILocationService _locationService;
        private IMoodService _moodService;
        private IFaceRecognitionService _faceRecognitionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
      

        /// <summary>
        /// Moods constructor
        /// </summary>
        /// <param name="locationService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="faceRecognitionService"></param>
        /// <param name="moodService"></param>
        public MoodsController(ILocationService locationService, IHttpContextAccessor httpContextAccessor, IFaceRecognitionService faceRecognitionService, IMoodService moodService)
        {
            _locationService = locationService;
            _httpContextAccessor = httpContextAccessor;
            _faceRecognitionService = faceRecognitionService;
            _moodService = moodService;
        }
 

        /// <summary>
        /// API Endpoint to capture the mood of uploaded image, identify logged in user location via its ip address
        /// </summary>
        /// <param name="request">Mood Capture Request</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("mood-capture")]
        
        public async Task<IActionResult> UploadMoodCaptureAsync([FromForm] MoodCaptureRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                request.UserId = userId;
            }

            using (var memoryStream = new MemoryStream())
            {
                await request.Image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                var mood = await _faceRecognitionService.AnalyzeImageEmotionAsync(memoryStream);
                request.Mood = mood;
                memoryStream.Dispose();
                
            }
            MoodCapture moodCapture = MoodMapper.MapMoodCaptureRequstToMoodCapture(request);
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var userLocation = _locationService.GetUserLocation(ipAddress);
            if (userLocation != null)
            {
                if (moodCapture.Location == null)
                {
                    moodCapture.Location = new Models.Location();
                }

                // added  hard coded values as the location service won't work from the same machine, to test this feature we can use ngrok and send the request from diffrent machine
                //to work with ngrok kept the https middleware commented

                moodCapture.Location.Latitude = userLocation.Result.Latitude != 0 ? userLocation.Result.Latitude : 39.2183;
                moodCapture.Location.Longitude = userLocation.Result.Longitude != 0 ? userLocation.Result.Longitude : 89.5130;
                moodCapture.Location.City = userLocation.Result.City != null ? userLocation.Result.City :"Delhi" ;
            }
            await  _moodService.InsertMoodCapture(moodCapture); 
            return Ok($"Image mood {moodCapture.Mood} captured successfully.");
        }

        /// <summary>
        /// API endpoint to get the mood frequency of logged in user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("frequency")]
        public async Task<IActionResult> GetMoodFrequency()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                var userMoodFrequency = await _moodService.GetMoodFrequencyDistribution(userId);
                return Ok(userMoodFrequency);
            }
            return BadRequest("Some error occured");
        }
            
    }
}
