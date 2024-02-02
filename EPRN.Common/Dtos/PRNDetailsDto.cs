using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class PRNDetailsDto
    {
        public string ReferenceNumber { get; set; }

        public string CreatedBy { get; set; }

        public string AccreditationNumber { get; set; }

        public string SiteAddress { get; set; }

        public bool DecemberWasteBalance { get; set; }

        public double? Tonnage { get; set; }

        public string SentTo { get; set; }

        public string Note { get; set; }

        public IEnumerable<PRNHistoryDto> History { get; set; }
    }
}
