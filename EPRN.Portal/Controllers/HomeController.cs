using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class HomeController : Controller
    {
        private IHomeService _homeService;

        public HomeController(IHomeServiceFactory homeServiceFactory)
        {
            if (homeServiceFactory == null) throw new ArgumentNullException(nameof(homeServiceFactory));
            _homeService = homeServiceFactory.CreateHomeService();
            if (_homeService == null) throw new ArgumentNullException(nameof(_homeService));

        }

        public async Task<IActionResult> Index()
        {
            var vm = await _homeService.GetHomePage();
            return View(vm);
        }

    }
}