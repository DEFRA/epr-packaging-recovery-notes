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
        private readonly IQuarterlyDatesService _quarterlyDatesService;

        public WasteController(IWasteService wasteService, IQuarterlyDatesService quarterlyDatesService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
            _quarterlyDatesService = quarterlyDatesService ?? throw new ArgumentNullException(nameof(quarterlyDatesService));
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
