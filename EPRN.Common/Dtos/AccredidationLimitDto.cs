namespace EPRN.Common.Dtos
{
    public class AccredidationLimitDto
    {
        public int JourneyId { get; internal set; }
        public double AccredidationLimit { get; internal set; }
        public double TotalToDate { get; internal set; }
        public double NewAmountEntered { get; internal set; }
        public double ExcessOfLimit { get; internal set; }
    }
}
