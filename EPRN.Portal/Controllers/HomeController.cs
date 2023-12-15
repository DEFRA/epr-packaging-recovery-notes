using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class HomeController : Controller
    {
        public UserRole UserRole { get; set; }
        private readonly IHomeServiceFactory _homeServiceFactory;
        private IHomeService _homeService;

        public HomeController(IHomeServiceFactory homeServiceFactory)
        {
            _homeServiceFactory = homeServiceFactory ?? throw new ArgumentNullException(nameof(homeServiceFactory));
            _homeService = _homeServiceFactory.CreateHomeService(UserRole);
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _homeService.GetHomePage();
            return View(vm);
        }

    }
}