﻿using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static EPRN.Common.Constants.Strings;

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
                return NotFound();
            
            var viewModel = await _prnService.GetDraftPrnConfirmationModel(id.Value);

            return View(viewModel);
        }

        [HttpGet]
        [ActionName(Routes.Actions.Prns.View)]
        public async Task<IActionResult> ViewPRN(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _prnService.GetViewPrnViewModel(id.Value);

            return View(viewModel);
        }
    }
}
