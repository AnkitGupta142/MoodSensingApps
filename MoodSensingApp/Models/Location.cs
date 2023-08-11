using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string  City { get; set; }
        // one location multiple mood captures
        public List<MoodCapture> MoodCaptures { get; set; }
    }
}
