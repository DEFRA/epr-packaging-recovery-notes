using EPRN.Portal.Constants;
using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class PRNSController : Controller
    {
        private IPRNService _prnService;

        public WasteType WasteType { get; set; }

        public PRNSController(IPRNService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        public IActionResult Tonnage(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = _prnService.GetTonnesViewModel(id.Value);

            return View();
        }
    }
}
