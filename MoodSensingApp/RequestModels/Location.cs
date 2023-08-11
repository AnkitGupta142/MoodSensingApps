using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.RequestModels
{
    public class Location
    {
        public double longitude { get; set; }

        public double lattitude { get; set; }

        public string  city { get; set; }
    }
}
