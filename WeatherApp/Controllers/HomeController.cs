using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Data;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{ 
    public class HomeController : Controller
    {
        private readonly WeatherDbContext _context;
        public HomeController(WeatherDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("api/weather")]
        [HttpGet]
        public IEnumerable<WeatherRecord> GetWeatherRecords()
        {
            return _context.WeatherRecord.ToList();
        }
    }
}
