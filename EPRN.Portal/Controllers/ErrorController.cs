using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EPRN.Portal.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        [Route("{statusCode:int}")]
        public IActionResult Index(int statusCode)
        {
            HttpContext.Response.StatusCode = statusCode;

            if (statusCode == (int)HttpStatusCode.Unauthorized ||
                statusCode == (int)HttpStatusCode.NotFound)
            {
                return View("PageNotFound");
            }

            if (statusCode == (int)HttpStatusCode.Forbidden)
            {
                return View("PageForbidden");
            }

            return View("Index", new ErrorViewModel
            {
                StatusCode = (HttpStatusCode)statusCode
            });
        }
    }
}