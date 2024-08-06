using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Data;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Service
{
    public class WeatherServiceBusiness : IWeatherServiceBusiness
    {
        private readonly ILogger<WeatherServiceBusiness> _logger;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        public WeatherServiceBusiness(ILogger<WeatherServiceBusiness> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _httpClient = new HttpClient();
            _configuration = configuration;
        }
        public async Task UpdateWeatherDataAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetService<WeatherDbContext>();

                    var apiKey = _configuration["ApiConfiguration:WeatherApiKey"];
                    if (string.IsNullOrEmpty(apiKey))
                    {
                        _logger.LogError("Weather API key is not configured.");
                        return;
                    }

                    var cities = new[] { "London", "Manchester", "New York", "Los Angeles", "Tokyo", "Osaka" };

                    foreach (var city in cities)
                    {
                        var response = await _httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}");
                        var weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);

                        var weatherRecord = new WeatherRecord
                        {
                            Country = weatherData.sys.country,
                            City = weatherData.name,
                            MinTemperature = weatherData.main.temp_min,
                            MaxTemperature = weatherData.main.temp_max,
                            LastUpdate = DateTime.UtcNow
                        }; 
                        
                        var existingRecord = await _context.WeatherRecord
                        .FirstOrDefaultAsync(w => w.City == city);

                        if (existingRecord != null)
                        {
                            existingRecord.MinTemperature = weatherRecord.MinTemperature;
                            existingRecord.MaxTemperature = weatherRecord.MaxTemperature;
                            existingRecord.LastUpdate = weatherRecord.LastUpdate;
                            _context.WeatherRecord.Update(existingRecord);
                        }
                        else
                        {
                            _context.WeatherRecord.Add(weatherRecord);
                        } 
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating weather data.");
            }
        }
        public async Task<IEnumerable<WeatherRecord>> GetWeatherDataAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetService<WeatherDbContext>();

                    if (_context == null)
                    {
                        _logger.LogError("WeatherDbContext could not be resolved.");
                        return Enumerable.Empty<WeatherRecord>(); // Return an empty list.
                    }

                    return await _context.WeatherRecord.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching weather data.");
                return Enumerable.Empty<WeatherRecord>(); // Return an empty list in case of an exception
            }
        }

    }
}
