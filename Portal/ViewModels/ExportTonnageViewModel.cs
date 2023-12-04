using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels
{
    public class ExportTonnageViewModel
    {
        public int JourneyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "MissingTonnageError")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "TonnesNotInRange")]
        public double? ExportTonnes { get; set; }
    }
}