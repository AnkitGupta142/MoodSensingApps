using System.IO;
using System.Threading.Tasks;

namespace MoodSensingApp.Services
{
    public interface IImageAnalyzerService
    {
        Task<string> AnalyzeImageEmotionAsync(Stream imageStream);
    }
}