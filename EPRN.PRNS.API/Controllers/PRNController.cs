using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Common.Dtos;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.PRNS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{id}/Category/{category}")]
    public class PRNController : Controller
    {
        private const string idParameter = "id";
        private const string categoryParameter = "category";
        private IPrnService _prnService;

        public PRNController(IPrnService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        [HttpPost]
        [Route("/api/[controller]/Create/Category/{category}/Material/{materialId}")]
        public async Task<IActionResult> CreatePrnRecord(
            int materialId,
            Category category)
        {
            var id = await _prnService.CreatePrnRecord(materialId, category);

            return Ok(id);
        }

        [HttpGet]
        [Route("Tonnage")]
        public async Task<IActionResult> GetTonnage(int id)
        {
            var tonnage = await _prnService.GetTonnage(id);

            return Ok(tonnage);
        }

        [HttpPost]
        [Route("Tonnage/{tonnage}")]
        public async Task<IActionResult> SaveTonnage(
            int id,
            double tonnage)
        {
            await _prnService.SaveTonnage(id, tonnage);

            return Ok();
        }

        [HttpGet]
        [Route("Confirmation")]
        public async Task<IActionResult> GetConfirmation(
            int id)
        {
            var confirmationDto = await _prnService.GetConfirmation(id);

            return Ok(confirmationDto);
        }

        [HttpGet]
        [Route("Check")]
        public async Task<IActionResult> CheckYourAnswers(
            int id)
        {
            var checkYourAnswersDto = await _prnService.GetCheckYourAnswers(id);

            return Ok(checkYourAnswersDto);
        }

        [HttpPost]
        [Route("Check")]
        public async Task<IActionResult> SaveCheckYourAnswersState(
            int id)
        {
            await _prnService.SaveCheckYourAnswers(id);

            return Ok();
        }

        [HttpGet]
        [Route("Status")]
        public async Task<IActionResult> GetStatus(
            int id)
        {
            return Ok(await _prnService.GetStatus(id));
        }

        [HttpGet]
        [Route("StatusAndProducer")]
        public async Task<IActionResult> GetStatusAndProducer(
            int id)
        {
            return Ok(await _prnService.GetStatusWithProducerName(id));
        }

        [HttpPost]
        [Route("Cancel")]
        public async Task<IActionResult> CancelPrn(
            int id,
            [FromBody] string reason)
        {
            await _prnService.CancelPrn(id, reason);

            return Ok();
        }

        [HttpPost]
        [Route("RequestCancel")]
        public async Task<IActionResult> RequestCancelPrn(
            int id,
            [FromBody] string reason)
        {
            await _prnService.RequestCancelPrn(id, reason);

            return Ok();
        }

        [HttpGet("/{page?}/{searchTerm?}/{filterBy?}/{sortBy?}")]
        public async Task<IActionResult> GetSentPrns([FromQuery] GetSentPrnsDto request)
        {
            var sentPrnsDto = await _prnService.GetSentPrns(request);
            return Ok(sentPrnsDto);
        }

        [HttpGet]
        [Route("/api/[controller]/Category/{category}/Details/{reference}")]
        public async Task<IActionResult> GetPrnDetails(string reference)
        {
            var prnDetailsDto = await _prnService.GetPrnDetails(reference);

            if (prnDetailsDto == null)
                return NotFound();

            return Ok(prnDetailsDto);
        }

        [HttpGet]
        [Route("DecemberWaste")]
        public async Task<ActionResult> GetDecemberWaste(int? id)
        {
            if (id == null)
                return BadRequest("Missing ID");
            
            var result = await _prnService.GetDecemberWaste(id.Value);

            return Ok(result);
        }

        [HttpPost]
        [Route("DecemberWaste/{decemberWaste}")]
        public async Task<ActionResult> SaveDecemberWaste(int? id, bool decemberWaste)
        {
            if (id == null)
                return BadRequest("Missing ID");

            await _prnService.SaveDecemberWaste(
                id.Value,
                decemberWaste);

            return Ok();
        }

        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> GetPrnDetails(int id)
        {
            var prnDetailsDto = await _prnService.GetPrnDetails(id);
            
            if (prnDetailsDto == null)
                return NotFound();

            return Ok(prnDetailsDto);
        }

        [HttpPost]
        [Route("SentTo/{sentTo}")]
        public async Task<ActionResult> SaveSentTo(int? id, string sentTo)
        {
            if (id == null)
                return BadRequest("Missing ID");

            await _prnService.SaveSentTo(
                id.Value,
                sentTo);

            return Ok();
        }

        [HttpPost]
        [Route("SaveDraftPrn")]
        public async Task<ActionResult> SaveDraftPrn(int? id)
        {
            if (id == null)
                return BadRequest("Missing ID");

            await _prnService.SaveDraftPrn(id.Value);

            return Ok();
        }

        /// <summary>
        /// Ensures that for every request a check is made that the record exists
        /// in the db
        /// </summary>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // if both id and category parameters exist, then we are retrieving a PRN in the usual journey
            // and therefore we need to exist the category is correct for the given ID. If a record is not
            // found for the given parameters, then we return a not found result without doing any further processing
            if (context.RouteData.Values.ContainsKey(idParameter) &&
                context.RouteData.Values.ContainsKey(categoryParameter))
            {
                int  id = Convert.ToInt32(context.RouteData.Values[idParameter]);
                if (Enum.TryParse<EPRN.Common.Enums.Category>(context.RouteData.Values["category"].ToString(), out var category))
                {
                    if (!await _prnService.PrnRecordExists(id, category))
                    {
                        context.Result = NotFound();
                        return;
                    }
                }
            }

            await next();
        }
    }
}