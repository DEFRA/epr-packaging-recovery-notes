using EPRN.Portal.Resources.PRNS;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class ViewSentPrnsViewModel
    {
        public IEnumerable<PrnRowViewModel> Rows { get; set; }

        public PaginationViewModel Pagination { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(ViewSentPrnResources))]
        public string SearchTerm { get; set; }
    }
}
