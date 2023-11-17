using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WasteManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WasteController : ControllerBase
    {
        private readonly ILogger<WasteController> _logger;
        private readonly IMapper _mapper;

        public WasteController(ILogger<WasteController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return null;
        }
    }
}