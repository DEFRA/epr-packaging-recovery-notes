using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class GetSentPrnsDto
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string SearchTerm { get; set; } = null;

        public PrnStatus? FilterBy { get; set; }

        public string SortBy { get; set; } = null;
    }
}
