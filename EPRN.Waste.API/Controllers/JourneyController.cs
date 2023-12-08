using EPRN.Common.Enums;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Waste.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JourneyController : Controller
    {
        private readonly IJourneyService _journeyService;

        public JourneyController(
            IJourneyService journeyService)
        {
            _journeyService = journeyService ?? throw new ArgumentNullException(nameof(journeyService));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<int> CreateJourney()
        {
            var journeyId = await _journeyService.CreateJourney();

            return journeyId;
        }

        [HttpGet]
        [Route("{journeyId}/WasteType")]
        public async Task<IActionResult> WasteType(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            return Ok(await _journeyService.GetWasteType(journeyId.Value));
        }

        [HttpPost]
        [Route("{journeyId}/Month/{selectedMonth}")]
        public async Task<IActionResult> SaveJourneyMonth(int? journeyId, int? selectedMonth)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (selectedMonth == null)
                return BadRequest("Selected month is missing");

            await _journeyService.SaveSelectedMonth(
                journeyId.Value,
                selectedMonth.Value);

            return Ok();
        }

        [HttpPost]
        [Route("{journeyId}/Type/{wasteTypeId}")]
        public async Task<IActionResult> SaveJourneyWasteType(int? journeyId, int? wasteTypeId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (wasteTypeId == null)
                return BadRequest("Waste type ID is missing");

            await _journeyService.SaveWasteType(
                journeyId.Value,
                wasteTypeId.Value);

            return Ok();
        }

        [HttpGet]
        [Route("Journey/{journeyId}/WhatHaveYouDoneWaste")]
        public async Task<ActionResult> GetWhatHaveYouDoneWaste(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            return Ok(await _journeyService.GetWhatHaveYouDoneWaste(journeyId.Value));
        }

        [HttpPost]
        [Route("{journeyId}/Done/{whatHaveYouDoneWaste}")]
        public async Task<IActionResult> SaveWhatHaveYouDoneWaste(int? journeyId, DoneWaste? whatHaveYouDoneWaste)
        {
            if (journeyId == null)
                return BadRequest("Journey Id is missing");

            if (whatHaveYouDoneWaste == null)
                return BadRequest("What Have You Done Waste is missing");

            await _journeyService.SaveWhatHaveYouDoneWaste(
                journeyId.Value,
                whatHaveYouDoneWaste.Value);

            return Ok();
        }

        [HttpGet]
        [Route("{journeyId}/status")]
        public async Task<IActionResult> GetWasteRecordStatus(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            var result = await _journeyService.GetWasteRecordStatus(journeyId.Value);

            if (result == null)
                return BadRequest($"No journey found with id: {journeyId}");

            return Ok(result);
        }

        [HttpPost]
        [Route("{journeyId}/Tonnage/{wasteTonnage}")]
        public async Task<IActionResult> SetJourneyTonnage(
            int? journeyId,
            double? wasteTonnage)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (wasteTonnage == null)
                return BadRequest("Journey Tonnage value is missing");

            await _journeyService.SaveTonnage(
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
            
            await _journeyService.SaveBaledWithWire(
                journeyId.Value, 
                baledWithWire.Value);

            return Ok();
        }

        [HttpPost]
        [Route("Journey/{journeyId}/ReProcessorExport/{siteId}")]
        public async Task<ActionResult> SaveReProcessorExport(int? journeyId, int? siteId)
        {
            if (journeyId == null)
                return BadRequest("Journey Id is missing");

            if (siteId == null)
                return BadRequest("SiteId is missing");

            //ToDo: This should be removed in the fullness of time.
            var id = await _wasteService.CreateJourney();

            await _wasteService.SaveReprocessorExport(id, siteId.Value);

            return Ok(id);
        }
    }
}
