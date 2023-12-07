using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels
{
    public class WasteTypesViewModel
    {
        public int JourneyId { get; set; }

        public Dictionary<int, string> WasteTypes { get; set; }

        [Required(ErrorMessageResourceName = "MissingWasteTypeSelectionMessage", ErrorMessageResourceType = typeof(WasteTypesResources))]
        public int? SelectedWasteTypeId { get; set; }
    }
}