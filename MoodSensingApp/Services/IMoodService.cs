using MoodSensingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoodSensingApp.Services
{
    public interface IMoodService
    {
        Task InsertMoodCapture(MoodCapture request);

        Task<Dictionary<string, int>> GetMoodFrequencyDistribution(int userId);

        Task<List<MoodCapture>> GetMoodCaptures(int userId, string mood);
    }
}