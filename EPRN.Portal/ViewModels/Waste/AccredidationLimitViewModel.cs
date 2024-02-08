using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.Waste
{
    public class AccredidationLimitViewModel
    {
        public UserRole UserRole { get; set; }
        public int Id { get; set; }
        public double AccredidationLimit { get; set; }
        public double TotalToDate { get; set; }
        public double NewAmountEntered { get; set; }
        public double ExcessOfLimit { get; set; }
        public string UserReferenceId { get; set; }
    }
}
