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
    }
}
