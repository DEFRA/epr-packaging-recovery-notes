namespace EPRN.Common.Dtos
{
    public class GetSentPrnsDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; } = null;
        public string FilterBy { get; set; } = null;
        public string SortBy { get; set; } = null;
    }
}
