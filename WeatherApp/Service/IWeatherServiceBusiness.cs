using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Service
{
    public interface IWeatherServiceBusiness
    {
        Task UpdateWeatherDataAsync();
        Task<IEnumerable<WeatherRecord>> GetWeatherDataAsync();
    }
}
