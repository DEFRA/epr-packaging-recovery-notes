using EPRN.Common.Enums;
using EPRN.Portal.Resources.PRNS;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class RequestCancelViewModel
    {
        public int Id { get; set; }

        public string Producer { get; set; }

        public string Regulator { get; set; }

        [Required(ErrorMessageResourceType = typeof(RequestCancelResources), ErrorMessageResourceName = "ReasonRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(RequestCancelResources), ErrorMessageResourceName = "MaxLengthMessage")]
        public string CancelReason { get; set; }

        public PrnStatus? Status { get; set; }

        public string Reference { get; set; }
    }
}
