using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class SentPrnsDto
    {
        public IEnumerable<PrnDto> Rows { get; set; }

        public PaginationDto Pagination { get; set; }

        public string SearchTerm { get; set; }

        public PrnStatus? FilterBy { get; set; }

        public string SortBy { get; set; }
    }
}
