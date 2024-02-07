using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class PRNHistoryDto
    {
        public string Username { get;set; }

        public DateTime Created {  get; set; }

        public PrnStatus Status { get; set; }

        public string Reason { get; set; }
    }
}