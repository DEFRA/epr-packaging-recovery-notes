using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class PrnsController : Controller
    {
        private readonly IPRNService _PRNService;

        public PrnsController(IPRNService PRNService)
        {
            _PRNService = PRNService;
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = await _PRNService.GetCreateViewModel();
            return View(viewModel);
        }
    }
}
