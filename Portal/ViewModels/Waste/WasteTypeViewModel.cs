using PRN.Common.Models;

namespace Portal.ViewModels.Waste
{
    public class WasteTypeViewModel
    {
        public int JourneyId { get; set; }

        public IEnumerable<WasteTypeDto> WasteTypes { get; set; } = null;

        public int? MaterialTypeId { get; set; }
    }
}