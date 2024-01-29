namespace EPRN.Common.Dtos
{
    public class GetSentPrnsDto
    {
        public int? Page { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public string? FilterBy { get; set; }
        public string? SortBy { get; set; }
    }
}
