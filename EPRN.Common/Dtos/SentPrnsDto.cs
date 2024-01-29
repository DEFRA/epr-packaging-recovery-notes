namespace EPRN.Common.Dtos
{
    public class SentPrnsDto
    {
        public List<PrnDto> Rows { get; set; }

        public PaginationDto Pagination { get; set; }
    }
}
