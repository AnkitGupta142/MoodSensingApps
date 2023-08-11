using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MoodSensingApp.Models;

namespace MoodSensingApp.Services
{
    public interface IFaceRecognitionService
    {
        Task<string> AnalyzeImageEmotionAsync(Stream imageStream);
    }
}