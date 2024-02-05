using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class ViewPRNViewModel
    {
        public string ReferenceNumber { get; set; }

        public string AccreditationNumber { get; set; }

        public string CreatedBy { get; set; }

        public string SiteAddress { get; set; }

        public bool DecemberWasteBalance { get; set; }

        public double? Tonnage { get; set; }

        public string SentTo { get; set; }

        // this is the date issued
        public DateTime? DateSent { get; set; }

        public string Note { get; set; }

        public PrnStatus Status { get; set; }

        public List<PRNHistoryViewModel> History { get; set; }
    }
}