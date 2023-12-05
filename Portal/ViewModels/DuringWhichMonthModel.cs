using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels
{
    public class DuringWhichMonthRequestViewModel
    {
        public int JourneyId { get; set; }

        public Dictionary<int, string> Quarter { get; set; } = new Dictionary<int, string>();

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(WhichQuarterResources))]
        public int? SelectedMonth { get; set; }

        public string WasteType { get; set; }
    }
}