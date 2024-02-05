using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class ViewSentPrnsViewModel
    {
        public IEnumerable<PrnRowViewModel> Rows { get; set; }

        public PaginationViewModel Pagination { get; set; }

        public string SearchTerm { get; set; }

        public string FilterBy { get; set; }

        public string SortBy { get; set; }

        public List<SelectListItem> SortItems { get; set; }

        public List<SelectListItem> FilterItems { get; set; } 
    }
}
