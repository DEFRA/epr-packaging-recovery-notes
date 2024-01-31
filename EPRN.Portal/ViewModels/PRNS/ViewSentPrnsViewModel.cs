using EPRN.Portal.Resources.PRNS;
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

        public List<SelectListItem> SortItems { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = @ViewSentPrnResources.SortBy }, // Default item
            new SelectListItem { Value = "1", Text = "Material" },
            new SelectListItem { Value = "2", Text = "Sent to" }
        };

        public List<SelectListItem> FilterItems { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = @ViewSentPrnResources.FilterBy }, // Default item
            new SelectListItem { Value = "3", Text = @ViewSentPrnResources.Accepted },
            new SelectListItem { Value = "6", Text = @ViewSentPrnResources.AwaitingAcceptance },
            new SelectListItem { Value = "7", Text = @ViewSentPrnResources.Rejected },
            new SelectListItem { Value = "8", Text = @ViewSentPrnResources.AwaitingCancellation },
            new SelectListItem { Value = "5", Text = @ViewSentPrnResources.Cancelled }
        };
    }
}
