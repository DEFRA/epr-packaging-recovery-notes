using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class WasteTypesViewModel
    {
        public int JourneyId { get; set; }

        public Dictionary<int, string>? WasteTypes { get; set; }

        [Required]
        public int? SelectedWasteTypeId { get; set; }
    }
}