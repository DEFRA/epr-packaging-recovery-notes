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
        public async Task<IActionResult> GetTonnage(int? id)
        {
            if (id == null)
                return BadRequest("Missing ID");

            var tonnage = await _prnService.GetTonnage(id.Value);

            return Ok(tonnage);
        }

        [HttpPost]
        [Route("Tonnage/{tonnage}")]
        public async Task<IActionResult> SaveTonnage(
            int? id,
            double? tonnage)
        {
            if (id == null)
                return BadRequest("Missing ID");

            if (tonnage == null)
                return BadRequest("Missong Tonnage");

            await _prnService.SaveTonnage(id.Value, tonnage.Value);

            return Ok();
        }

        [HttpGet]
        [Route("Confirmation")]
        public async Task<IActionResult> GetConfirmation(
            int? id)
        {
            if (id == null)
                return BadRequest("Missing ID");

            var confirmationDto = await _prnService.GetConfirmation(id.Value);

            return Ok(confirmationDto);
        }

        [HttpGet]
        [Route("Check")]
        public async Task<IActionResult> CheckYourAnswers(
            int? id)
        {
            if (id == null)
                return BadRequest("Missing ID");

            var checkYourAnswersDto = await _prnService.GetCheckYourAnswers(id.Value);

            return Ok(checkYourAnswersDto);
        }

        [HttpPost]
        [Route("Check")]
        public async Task<IActionResult> SaveCheckYourAnswersState(
            int? id)
        {
            if (id == null)
                return BadRequest("Missing ID");

            await _prnService.SaveCheckYourAnswers(id.Value);

            return Ok();
        }

        [HttpPost]
        [Route("Cancel")]
        public async Task<IActionResult> CancelPrn(
            int id,
            [FromBody]string reason)
        {
            await _prnService.CancelPrn(id, reason);

            return Ok();
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