using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.PRNS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{id}")]
    public class PRNController : ControllerBase
    {
        private IPrnService _prnService;

        public PRNController(IPrnService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        [HttpGet]
        [ActionName("Tonnage")]
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
    }
}
