using System;

namespace MoodSensingApp.Models
{
    public class MoodCapture
    {
        public int MoodCaptureId { get; set; }

        public int UserId { get; set; }
        public int LocationId { get; set; }

        public Location Location { get; set; }
        public int MoodValue { get; set; }

        public string Mood { get; set; }
        public DateTime Timestamp { get; set; }

        public User User { get; set; }
    }
}