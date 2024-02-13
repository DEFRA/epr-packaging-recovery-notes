using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using EPRN.Portal.Resources.PRNS;
using System.ComponentModel.DataAnnotations;
namespace EPRN.Portal.ViewModels.PRNS
{
    public class DestinationPrnViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(DestinationPRNResources))]
        public PrnStatus? DoWithPRN { get; set; }
    }
}