using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class TonnesViewModel
    {
        public int JourneyId { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "TonnesNotInRange")]
        public int? Tonnes { get; set; }
    }
}
