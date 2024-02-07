using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class DecemberWasteViewModel
    {
        public int Id { get; set; }

        public int materialId { get; set; }

        public Category Category { get; set; }
        
        public double BalanceAvailable { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(DecemberWasteResource))]
        public bool? WasteForDecember { get; set; }
    }
}