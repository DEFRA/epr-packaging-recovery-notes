using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using EPRN.Portal.Resources.PRNS;
using System.ComponentModel.DataAnnotations;
namespace EPRN.Portal.ViewModels.PRNS
{
    public class DraftConfirmationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(DraftConfirmationResources))]
        public PrnStatus? DoWithPRN { get; set; }
    }
}