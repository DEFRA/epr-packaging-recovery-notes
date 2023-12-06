using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Waste.API.Configuration;
using Waste.API.Configuration.Interfaces;
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

        [HttpGet]
        [Route("Journey/{journeyId}/WasteType")]
        public async Task<ActionResult> WasteType(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            return Ok(await _wasteService.GetWasteType(journeyId.Value));
        }

        [HttpPost]
        [Route("Journey/{journeyId}/Month/{selectedMonth}/WhatHaveYouDoneWaste/{whatHaveYouDoneWaste}")]
        public async Task<IActionResult> SaveJourneyMonth(int? journeyId, int? selectedMonth, DoneWaste? whatHaveYouDoneWaste)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (selectedMonth == null)
                return BadRequest("Selected month is missing");

            if (whatHaveYouDoneWaste == null)
                return BadRequest("What was done to the waste value is missing");

            //TODO: This should be removed in the fullness of time
            var id = await _wasteService.CreateJourney();
            await _wasteService.SaveSelectedMonth(id, selectedMonth.Value, whatHaveYouDoneWaste.Value);

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
        [Route("Journey/{journeyId}/Done/{whatHaveYouDoneWaste}")]
        public async Task<ActionResult> SaveWhatHaveYouDoneWaste(int? journeyId, DoneWaste? whatHaveYouDoneWaste)
        {
            if (journeyId == null)
                return BadRequest("Journey Id is missing");

            if (whatHaveYouDoneWaste == null)
                return BadRequest("What Have You Done Waste is missing");

            //ToDo: This should be removed in the fullness of time.
            var id = await _wasteService.CreateJourney();
            await _wasteService.SaveWhatHaveYouDoneWaste(id, whatHaveYouDoneWaste.Value);

            return Ok(id);
        }

        [HttpGet]
        [Route("Journey/{journeyId}/status")]
        public async Task<IActionResult> GetWasteRecordStatus(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            var result = await _wasteService.GetWasteRecordStatus(journeyId.Value);

            if (result == null)
                return BadRequest($"No journey found with id: {journeyId}");

            return Ok(result);
        }

        [HttpPost]
        [Route("Journey/{journeyId}/Tonnage/{wasteTonnage}")]
        public async Task<IActionResult> SetJourneyTonnage(
            int? journeyId,
            double? wasteTonnage)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (wasteTonnage == null)
                return BadRequest("Journey Tonnage value is missing");

            await _wasteService.SaveTonnage(
                journeyId.Value,
                wasteTonnage.Value);

            return Ok();
        }
        [HttpPost]
        [Route("Journey/{journeyId}/BaledWithWire/{baledWithWire}")]
        public async Task<ActionResult> SaveBaledWithWire(int? journeyId, bool? baledWithWire)
        {
            if (journeyId == null)
                return BadRequest("Journey Id is missing");

            if (baledWithWire == null)
                return BadRequest("Baled with wire is missing");
            
            //ToDo: This should be removed in the fullness of time.
            var id = await _wasteService.CreateJourney();

            await _wasteService.SaveBaledWithWire(id, baledWithWire.Value);

            return Ok(id);
        }

    }
}
