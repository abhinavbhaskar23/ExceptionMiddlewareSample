using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace ExceptionMiddlewareSample.Controllers
{
  [Produces("application/vnd.hal+json")]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
      _logger = logger;
    }

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetError(string id = null)
    //{
    //  if(id == null)
    //    BadRequest();

    //  return Ok("");
    //}

    [Produces("application/vnd.hal+json", "application / json")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      //BadRequest();
      throw new Exception("This is a new test exception");
      var rng = new Random();
      IEnumerable<WeatherForecast> returnVal = Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      })
      .ToArray();

      return Ok(returnVal);
    }

    [Produces("application/vnd.hal+json", "application / json")]
    [Route("GetNew/{id}"), HttpGet]
    public async Task<IActionResult> GetNew(string id="1")
    {
      //BadRequest();
      throw new Exception("This is a new test exception22222222222222222");
      var rng = new Random();
      IEnumerable<WeatherForecast> returnVal = Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      })
      .ToArray();

      return Ok(returnVal);
    }

    //[HttpGet]
    //public async Task<IActionResult> GetCustomer()
    //{
    //  //throw new Exception("This is a new test exception");
    //  var rng = new Random();
    //  IEnumerable<WeatherForecast> returnVal = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //  {
    //    Date = DateTime.Now.AddDays(index),
    //    TemperatureC = rng.Next(-20, 55),
    //    Summary = Summaries[rng.Next(Summaries.Length)]
    //  })
    //  .ToArray();

    //  return Ok(returnVal);
    //}
  }
}
