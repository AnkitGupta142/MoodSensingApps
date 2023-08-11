using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string Salt { get; set; }

        //one user multiple moods
        public List<MoodCapture> MoodCaptures { get; set; }
        public List<MoodFrequency> MoodFrequencies { get; set; }
    }
}
