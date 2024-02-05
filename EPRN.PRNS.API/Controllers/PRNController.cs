using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.PRNS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{id}")]
    public class PRNController : Controller
    {
        private const string idParameter = "id";
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

        [HttpGet]
        [Route("/api/[controller]/Details/{reference}")]
        public async Task<IActionResult> GetPrnDetails(string reference)
        {
            var prnDetailsDto = await _prnService.GetPrnDetails(reference);

            if (prnDetailsDto == null)
                return NotFound();

            return Ok(prnDetailsDto);
        }

        /// <summary>
        /// Ensures that for every request a check is made that the record exists
        /// in the db
        /// </summary>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           if (context.ActionDescriptor.Parameters.Any(p => p.Name == idParameter) &&
                context.ActionArguments.ContainsKey(idParameter))
            {
                int id = Convert.ToInt32(context.ActionArguments[idParameter]);

                if (!await _prnService.PrnRecordExists(id))
                {
                    context.Result = NotFound();
                    return;
                }
            }

            await next();
        }
    }
}