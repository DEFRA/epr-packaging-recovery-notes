using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class BaledWithWireModel
    {
        public int JourneyId { get; set; }

        public string? WasteType { get; set; }

        public bool? BaledWithWire { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(BaledWithWireResource))]
        public string BaledWithWireScreen { get; set; }
    }
}