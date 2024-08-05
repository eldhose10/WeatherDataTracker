using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Models;

namespace WeatherApp.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext()
        {
        }
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }
        public DbSet<WeatherRecord> WeatherRecord { get; set; }
    }
}
