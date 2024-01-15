using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class PrnsController : BaseController
    {
        private readonly IPRNService _PRNService;

        public PrnsController(Func<Category, IPRNService> prnServiceFactory)
        {
            _PRNService = prnServiceFactory.Invoke(Category.Exporter);
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = await _PRNService.CreatePrnViewModel();
            return View(viewModel);
        }
    }
}
