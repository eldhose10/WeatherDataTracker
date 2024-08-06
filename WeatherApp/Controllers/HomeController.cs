using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using WeatherApp.Models;
using WeatherApp.Service;

namespace WeatherApp.Controllers
{ 
    public class HomeController : Controller
    {
        private readonly IWeatherServiceBusiness _weatherBusiness;
        public HomeController(IWeatherServiceBusiness weatherBusiness)
        {
            _weatherBusiness = weatherBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("api/weather")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherRecord>>> GetWeatherRecords()
        {
            var records = await _weatherBusiness.GetWeatherDataAsync();
            if (records == null)
            {
                return NotFound(); 
            }
            return Ok(records);
        }
    }
}
