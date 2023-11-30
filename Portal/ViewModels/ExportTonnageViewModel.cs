using Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class ExportTonnageViewModel
    {
        public int JourneyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "MissingTonnageError")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "TonnesNotInRange")]
        public int? ExportTonnes { get; set; }
    }
}