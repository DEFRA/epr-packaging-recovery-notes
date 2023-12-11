using EPRN.Common.Dtos;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Waste.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WasteController : ControllerBase
    {
        private readonly IWasteService _wasteService;

        public WasteController(IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        [HttpGet]
        [Route("Types")]
        public async Task<IEnumerable<WasteTypeDto>> WasteTypes()
        {
            return await _wasteService.WasteTypes();
        }

        [HttpGet]
        [Route("Types/{wasteTypeid}/SubTypes")]
        public async Task<IEnumerable<WasteSubTypeDto>> WasteSubTypes(int wasteTypeid)
        {
            return await _wasteService.WasteSubTypes(wasteTypeid);
        }
    }
}
