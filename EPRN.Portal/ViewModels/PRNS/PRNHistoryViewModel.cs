using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class PRNHistoryViewModel
    {
        public PrnStatus Status { get; set; }

        public string Username { get; set; }

        public DateTime Created {  get; set; }

        public string Reason { get; set; }
    }
}
