using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EPRN.Portal.Controllers
{
    public class BaseController<T> : Controller
    {
        protected readonly ILogger<T> logger;

        public BaseController(ILogger<T> logger) 
        {
            this.logger = logger;
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
