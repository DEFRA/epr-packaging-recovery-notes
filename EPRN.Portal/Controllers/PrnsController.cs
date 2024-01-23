using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class PrnsController : BaseController
    {
        private readonly IPRNService _prnService;

        public PrnsController(IPRNService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        public async Task<IActionResult> Create(int? id)
        {
            var viewModel = await _prnService.CreatePrnViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PrnSavedAsDraftConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else if (id <= 0)
            {
                return BadRequest();
            }

            var viewModel = await _prnService.GetDraftPrnConfirmationModel(id.Value);

            return View(viewModel);
        }
    }
}
