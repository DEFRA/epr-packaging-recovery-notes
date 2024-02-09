using EPRN.Common.Enums;
using EPRN.Portal.Resources.PRNS;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class CancelViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(CancelResources), ErrorMessageResourceName = "ReasonRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(CancelResources), ErrorMessageResourceName = "MaxLengthMessage")]
        public string CancelReason { get; set; }

        public string Reference { get; set; }

        public string Producer { get; set; }

        public PrnStatus? Status { get; set; }
    }
}