using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class WeatherRecord
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
