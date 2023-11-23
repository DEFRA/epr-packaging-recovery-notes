using EPRN.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Waste.API.Services.Interfaces;

namespace Waste.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WasteController : ControllerBase
    {
        public readonly IWasteService _wasteService;

        public WasteController(IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        [HttpGet]
        [Route("WasteTypes")]
        // add route and cal lmethod GetMaterialTypes
        public async Task<IEnumerable<WasteTypeDto>> WasteTypes()
        {
            return await _wasteService.WasteTypes();
        }

        [HttpPost]
        [Route("Journey/{journeyId}/Month/{selectedMonth}")]
        public async Task<IActionResult> SaveJourneyMonth(int? journeyId, int? selectedMonth)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (selectedMonth == null)
                return BadRequest("Selected month is missing");

            //TODO: This should be removed in the fullness of time
            var id = await _wasteService.CreateJourney();
            await _wasteService.SaveSelectedMonth(id, selectedMonth.Value);

            return Ok();
        }

        [HttpPost]
        [Route("Journey/{journeyId}/Type/{wasteTypeId}")]
        public async Task<IActionResult> SaveJourneyWasteType(int? journeyId, int? wasteTypeId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (wasteTypeId == null)
                return BadRequest("Waste type ID is missing");

            //TODO: This should be removed in the fullness of time
            var id = await _wasteService.CreateJourney();
            await _wasteService.SaveWasteType(id, wasteTypeId.Value);

            return Ok();
        }
        
        [HttpPost]
        [Route("Journey/{journeyId}/WasteType/{selectedWasteType}")]
        public async Task<ActionResult> SaveWasteType(int? journeyId, string? selectedWasteType)
        {
            if (journeyId == null)
                return BadRequest("Journey Id is missing");

            if (selectedWasteType == null)
                return BadRequest("Selected waste is missing");

            //ToDo: This should be removed in the fullness of time.
            var id = await _wasteService.CreateJourney();
            await _wasteService.SaveSelectedWasteType(id, selectedWasteType);

            return Ok(id);
        }
    }
}
