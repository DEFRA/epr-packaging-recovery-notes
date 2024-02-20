using EPRN.Common.Constants;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using static EPRN.Common.Constants.Strings;
using Routes = EPRN.Common.Constants.Strings.Routes;

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
        [ActionName(Routes.Actions.PRNS.View)]
        [Route("[controller]/[action]/{reference}")]
        public async Task<IActionResult> ViewPRN(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                return NotFound();

            var viewModel = await _prnService.GetViewPrnViewModel(reference);

            return View(viewModel);
        }

        [HttpGet]
        [ActionName(Routes.Actions.PRNS.ViewSentPrns)]
        public async Task<IActionResult> ViewSentPrns([FromQuery] GetSentPrnsViewModel request)
        {
            var viewSentPrnsViewModel = await _prnService.GetViewSentPrnsViewModel(request);

            return View(viewSentPrnsViewModel);
        }


    }
}