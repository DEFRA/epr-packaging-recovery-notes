using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.Waste
{
    public class AccredidationLimitViewModel
    {
        public UserRole UserRole { get; internal set; }
        public int JourneyId { get; internal set; }
        public double AccredidationLimit { get; internal set; }
        public double TotalToDate { get; internal set; }
        public double NewAmountEntered { get; internal set; }
        public double ExcessOfLimit { get; internal set; }
        public string UserReferenceId { get; set; }
    }
}
