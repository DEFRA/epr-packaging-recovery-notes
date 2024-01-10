namespace EPRN.Common.Dtos
{
    public class GetBaledWithWireDto
    {
        public int JourneyId { get; set; }
        public bool?  BaledWithWire { get; set; }
        public double? BaledWithWireDeductionPercentage { get; set; }
    }
}
