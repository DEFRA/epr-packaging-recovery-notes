using EPRN.Portal.Constants;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public abstract class JourneyController : Controller
    {
        public JourneyType JourneyType { get; set; }
    }
}
