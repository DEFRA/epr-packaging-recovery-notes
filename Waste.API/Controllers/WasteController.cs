using Microsoft.AspNetCore.Mvc;
using PRN.Common.Models;
using Waste.API.Services.Interfaces;

namespace WasteManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WasteController : ControllerBase
    {
        private readonly ILogger<WasteController> _logger;
        private readonly IWasteService _wasteService;

        public WasteController(
            ILogger<WasteController> logger, 
            IWasteService wasteService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        [HttpGet]
        [Route("Types")]
        public IEnumerable<WasteTypeDto> GetTypes()
        {
            return _wasteService.GetWasteTypes();
        }
    }
}