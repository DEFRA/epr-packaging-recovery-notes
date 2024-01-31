namespace EPRN.Common.Dtos
{
    public class GetSentPrnsViewModel
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 2;

        public string SearchTerm { get; set; } = null;

        public string FilterBy { get; set; } = null;

        public string SortBy { get; set; } = null;
    }
}
