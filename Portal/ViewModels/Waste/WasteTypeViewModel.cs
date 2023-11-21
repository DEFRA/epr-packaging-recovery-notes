using Portal.Resources;
using EPRN.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels.Waste
{
    public class WasteTypeViewModel
    {
        public int JourneyId { get; set; }

        public IEnumerable<WasteTypeDto> WasteTypes { get; set; }

        [Required(ErrorMessageResourceType = typeof(WasteResources), ErrorMessageResourceName = "MaterialTypeMissingMessage")]
        public int? MaterialTypeId { get; set; }
    }
}