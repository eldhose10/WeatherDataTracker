using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Service
{
    public interface IWeatherServiceBusiness
    {
        Task UpdateWeatherDataAsync();
    }
}
