using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class ReProcessorExportViewModel
    {
        public int JourneyId { get; set; }

//        public string WasteType { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(ReProcessorExportResource))]
        public int? SelectedSite { get; set; }
    }
}
