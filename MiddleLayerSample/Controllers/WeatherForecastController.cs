using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
      throw new Exception("This is a new test exception");
    }

    [Produces("application/vnd.hal+json", "application / json")]
    [Route("GetNew/{id}"), HttpGet]
    public async Task<IActionResult> GetNew(string id="1")
    {
      throw new Exception("This is a another test exception");
    }
  }
}
