using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class PrnRowViewModel
    {
        public string PrnNumber { get; set; }
        public string Material { get; set; }
        public string SentTo { get; set; }
        public string DateCreated { get; set; }
        public double? Tonnes { get; set; }
        public PrnStatus Status { get; set; }
        public string Link { get; set; }

        public string GetCssClassForStatus()
        {
            switch (Status)
            {
                case PrnStatus.Accepted:
                    return "green";
                case PrnStatus.AwaitingAcceptance:
                    return "blue";
                case PrnStatus.Rejected:
                    return "red";
                case PrnStatus.AwaitingCancellation:
                    return "purple";
                case PrnStatus.Cancelled:
                    return "yellow";
            }
            return null;
        }
    }
}