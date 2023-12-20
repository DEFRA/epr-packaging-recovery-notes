using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.PRNS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PRNController : ControllerBase
    {
        public PRNController(IPrnService prnService)
        {
            
        }
    }
}
