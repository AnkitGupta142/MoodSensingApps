using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Models
{
    public class MoodFrequency
    {
        public int MoodFrequencyId { get; set; }
        public int UserId { get; set; }
        public string Mood { get; set; }
        public int Frequency { get; set; }

        public User User { get; set; }

    }
}
