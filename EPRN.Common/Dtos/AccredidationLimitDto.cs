namespace EPRN.Common.Dtos
{
    public class AccredidationLimitDto
    {
        public int JourneyId { get; set; }
        public string UserReferenceId { get; set; }

        public double AccredidationLimit { get; set; }
        public double TotalToDate { get; set; }
        public double NewAmountEntered { get; set; }
        public double ExcessOfLimit { get; set; }
    }
}
