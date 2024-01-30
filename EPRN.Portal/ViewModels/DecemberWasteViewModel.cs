using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class DecemberWasteViewModel
    {
        public int JourneyId { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(DecemberWasteResource))]
        public bool? DecemberWaste { get; set; }
    }
}