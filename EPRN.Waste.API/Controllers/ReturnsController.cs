using EPRN.Waste.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Waste.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReturnsController : ControllerBase
    {
        private readonly IQuarterlyDatesService _quarterlyDatesService;

        public ReturnsController(IWasteService wasteService, IQuarterlyDatesService quarterlyDatesService)
        {   
            _quarterlyDatesService = quarterlyDatesService ?? throw new ArgumentNullException(nameof(quarterlyDatesService));
        }

        [HttpGet]
        [Route("QuarterlyDates")]
        public async Task<Dictionary<int, string>> GetActiveQuarterlyDates(DateTime currentDate, bool hasSubmittedPreviousQuarterReturn)
        {
            return await _quarterlyDatesService.GetQuarterMonthsToDisplay(currentDate, hasSubmittedPreviousQuarterReturn);
        }
    }
}
