using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Waste.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{journeyId}")]
    public class JourneyController : BaseController
    {
        private readonly IQuarterlyDatesService _quarterlyDatesService;

        public JourneyController(
            IJourneyService journeyService, IQuarterlyDatesService quarterlyDatesService) : base(journeyService)
        {
            _quarterlyDatesService = quarterlyDatesService ?? throw new ArgumentNullException(nameof(quarterlyDatesService));
        }

        [HttpPost]
        [Route("/api/[controller]/Create/Material/{materialId}/Category/{category}")]
        public async Task<IActionResult> CreateJourney(
            int? materialId,
            Category? category)
        {
            if (materialId == null)
                return BadRequest("Material ID is missing");

            if (category == null)
                return BadRequest("Category is missing");

            var journeyId = await _journeyService.CreateJourney(
                materialId.Value, 
                category.Value);

            return Ok(journeyId);
        }

        [HttpGet]
        [Route("WasteType")]
        public async Task<IActionResult> WasteType(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            return Ok(await _journeyService.GetWasteType(journeyId.Value));
        }

        [HttpGet]
        [Route("QuarterlyDates/{currentDate}/{hasSubmittedPreviousQuarterReturn}")]
        public async Task<IActionResult> GetActiveQuarterlyDates(int? journeyId, int currentDate, bool hasSubmittedPreviousQuarterReturn)
        {
            var result =
                await _quarterlyDatesService.GetQuarterMonthsToDisplay(currentDate, hasSubmittedPreviousQuarterReturn);
            
            return Ok(result);
        }

        [HttpGet]
        [Route("Month")]
        public async Task<IActionResult> GetSelectedMonth(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            var selectdMonth = await _journeyService.GetSelectedMonth(journeyId.Value);

            return Ok(selectdMonth);
        }

        [HttpPost]
        [Route("Month/{selectedMonth}")]
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

        [HttpGet]
        [Route("Type")]
        public async Task<IActionResult> GetWasteTypeId(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            return Ok(await _journeyService.GetWasteTypeId(journeyId.Value));
        }

        [HttpGet]
        [Route("Subtype")]
        public async Task<IActionResult> GetWasteSubTypeId(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            return Ok(await _journeyService.GetWasteSubTypeSelection(journeyId.Value));
        }

        [HttpPost]
        [Route("Type/{wasteTypeId}")]
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

        [HttpPost]
        [Route("SubTypes/{wasteSubTypeId}/Adjustment/{adjustment}")]
        public async Task<IActionResult> SaveJourneyWasteType(int? journeyId, int? wasteSubTypeId, double? adjustment)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (wasteSubTypeId == null)
                return BadRequest("Waste sub type ID is missing");

            if (adjustment == null)
                return BadRequest("Adjustment is missing");

            await _journeyService.SaveWasteSubType(
                journeyId.Value,
                wasteSubTypeId.Value,
                adjustment.Value);

            return Ok();
        }

        [HttpGet]
        [Route("WhatHaveYouDoneWaste")]
        public async Task<ActionResult> GetWhatHaveYouDoneWaste(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            return Ok(await _journeyService.GetWhatHaveYouDoneWaste(journeyId.Value));
        }

        [HttpPost]
        [Route("Done/{whatHaveYouDoneWaste}")]
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
        [Route("status")]
        public async Task<IActionResult> GetWasteRecordStatus(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            var result = await _journeyService.GetWasteRecordStatus(journeyId.Value);

            if (result == null)
                return BadRequest($"No journey found with id: {journeyId}");

            return Ok(result);
        }

        [HttpGet]
        [Route("Tonnage")]
        public async Task<IActionResult> GetJourneyTonnage(
            int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            var tonnage = await _journeyService.GetTonnage(journeyId.Value);

            return Ok(tonnage);
        }

        [HttpPost]
        [Route("Tonnage/{wasteTonnage}")]
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
        [Route("ReProcessorExport/{siteId}")]
        public async Task<ActionResult> SaveReProcessorExport(int? journeyId, int? siteId)
        {
            if (journeyId == null)
                return BadRequest("Journey Id is missing");

            if (siteId == null)
                return BadRequest("SiteId is missing");

            await _journeyService.SaveReprocessorExport(journeyId.Value, siteId.Value);

            return Ok();
        }

        [HttpGet]
        [Route("Note")]
        public async Task<IActionResult> GetWasteRecordNote(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            var result = await _journeyService.GetWasteRecordNote(journeyId.Value);

            return Ok(result);
        }

        [HttpPost]
        [Route("Note/{note}")]
        public async Task<IActionResult> SaveWasteRecordNote(int? journeyId, string note)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (string.IsNullOrWhiteSpace(note))
                return Ok();

            await _journeyService.SaveWasteRecordNote(journeyId.Value, note);

            return Ok();
        }

        [HttpGet]
        [Route("BaledWithWire")]
        public async Task<ActionResult> GetBaledWithWire(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            BaledWithWireDto result = await _journeyService.GetBaledWithWire(journeyId.Value);

            return Ok(result);
        }

        [HttpPost]
        [Route("BaledWithWire/{baledWithWire}/{baledWithWireDeductionPercentage}")]
        public async Task<ActionResult> SaveBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage)
        {
            await _journeyService.SaveBaledWithWire(
                journeyId,
                baledWithWire,
                baledWithWireDeductionPercentage);

            return Ok();
        }

        [HttpGet]
        [Route("JourneyAnswers")]
        public async Task<IActionResult> GetJourneyAnswers(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            if (journeyId <= 0)
                return BadRequest("Invalid journey ID");

            JourneyAnswersDto dto = await _journeyService.GetJourneyAnswers(journeyId.Value);
            return Ok(dto);
        }

        [HttpGet]
        [Route("Category")]
        public async Task<IActionResult> GetJourneyCategory(
            int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");

            var category = await _journeyService.GetCategory(journeyId.Value);

            return Ok(category);
        }

        [HttpGet]
        [Route("DecemberWaste")]
        public async Task<ActionResult> GetDecemberWaste(int? journeyId)
        {
            if (journeyId == null)
                return BadRequest("Journey ID is missing");
            if (journeyId <= 0)
                return BadRequest("Invalid journey ID");

            DecemberWasteDto result = await _journeyService.GetDecemberWaste(journeyId.Value);

            return Ok(result);
        }

        [HttpPost]
        [Route("DecemberWaste/{decemberWaste}")]
        public async Task<ActionResult> SaveDecemberWaste(int journeyId, bool decemberWaste)
        {
            await _journeyService.SaveDecemberWaste(
                journeyId,
                decemberWaste);

            return Ok();
        }
    }
}
