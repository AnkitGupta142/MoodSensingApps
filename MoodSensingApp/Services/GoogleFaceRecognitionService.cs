using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using MoodSensingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Services
{
    public class GoogleFaceRecognitionService : IFaceRecognitionService
    {


        private readonly ImageAnnotatorClient _client;

        public GoogleFaceRecognitionService()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "vision_api_key.json");
            _client = ImageAnnotatorClient.Create();
        }

        public async Task<string> AnalyzeImageEmotionAsync(Stream imageStream)
        {
            try
            {
                var image = Image.FromStream(imageStream);
                var response = await _client.DetectFacesAsync(image);
               

                if (response.Count > 0)
                {
                    var jsonResponse = JsonConvert.SerializeObject(response[0]);
                    var mappedResponse =   JsonConvert.DeserializeObject<MoodAnalyzerClass>(jsonResponse);
                    var x = GetHighestLikelihood(mappedResponse);
                    return x;
                }
                else
                {
                    return "No Data found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string GetHighestLikelihood(MoodAnalyzerClass moodAnalyzer)
        {
            var likelihoods = new Dictionary<string, int>
        {
            { "Joy", moodAnalyzer.JoyLikelihood },
            { "Sorrow", moodAnalyzer.SorrowLikelihood },
            { "Anger", moodAnalyzer.AngerLikelihood },
            { "Surprise", moodAnalyzer.SurpriseLikelihood }
        };

            var maxLikelihood = likelihoods.OrderByDescending(kv => kv.Value).FirstOrDefault();
            return maxLikelihood.Key;
        }


    }
}
