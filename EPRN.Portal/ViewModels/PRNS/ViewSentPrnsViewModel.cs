namespace EPRN.Portal.ViewModels.PRNS
{
    public class ViewSentPrnsViewModel
    {
        public IEnumerable<PrnRowViewModel> Rows { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
