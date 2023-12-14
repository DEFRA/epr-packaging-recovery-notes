using EPRN.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomePageService _homePageService;

        public HomeController(IHomePageService homePageService)
        {
            _homePageService = homePageService ?? throw new ArgumentNullException(nameof(homePageService));
        }

        public IActionResult Index()
        {
            var homePageViewModel = _homePageService.GetHomepageViewModel();

            return View(homePageViewModel);
        }
    }
}