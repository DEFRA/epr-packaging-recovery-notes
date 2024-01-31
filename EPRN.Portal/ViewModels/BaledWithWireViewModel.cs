using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class BaledWithWireViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(BaledWithWireResource))]
        public bool? BaledWithWire { get; set; }

        public double? BaledWithWireDeductionPercentage { get; set; }
    }
}