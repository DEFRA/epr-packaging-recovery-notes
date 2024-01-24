using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class PrnRowViewModel
    {
        public string PrnNumber { get; set; }
        public string Material { get; set; }
        public string SentTo { get; set; }
        public string DateCreated { get; set; }
        public double Tonnes { get; set; }
        public PrnRecordStatus Status { get; set; }
        public string Link { get; set; }
    }
}
