using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Data;
using WeatherApp.Models;
using WeatherApp.Service;

namespace WeatherApp.Services
{
    public class WeatherUpdateHostedService : IHostedService, IDisposable
    {
        // private readonly WeatherDbContext _context;
        private Timer _timer = null;
        private IWeatherServiceBusiness _weatherBusiness;
        private readonly ILogger<WeatherUpdateHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        public WeatherUpdateHostedService(ILogger<WeatherUpdateHostedService> logger, IWeatherServiceBusiness weatherBusiness, IServiceProvider serviceProvider)//, WeatherDbContext context
        {
            _logger = logger;
            _weatherBusiness = weatherBusiness;
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {  
            // Calculate the time until the next occurrence, every 1 minutes
            TimeSpan timeUntilNextRun = TimeSpan.FromMinutes(1);

            // Schedule the timer to run every 1 minute
            _timer = new Timer(DoWork, null, TimeSpan.Zero, timeUntilNextRun);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                //Weather API Service is Running....... 

                using (var scope = _serviceProvider.CreateScope())
                {
                    _weatherBusiness.UpdateWeatherDataAsync();
                }
                //Weather API Service is Completed.......
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Weather API Hosted Service is stopping.....");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
}
