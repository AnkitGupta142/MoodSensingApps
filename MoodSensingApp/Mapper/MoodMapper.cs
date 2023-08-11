using MoodSensingApp.Models;
using MoodSensingApp.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Mapper
{
    public static class MoodMapper
    {
        public static MoodCapture MapMoodCaptureRequstToMoodCapture(MoodCaptureRequest moodCaptureRequest)
        {
            MoodCapture moodCapture = new MoodCapture();
            moodCapture.UserId = moodCaptureRequest.UserId;
            moodCapture.Mood= moodCaptureRequest.Mood;
            moodCapture.Timestamp = DateTime.Now;
            return moodCapture;
        }
    }
}
