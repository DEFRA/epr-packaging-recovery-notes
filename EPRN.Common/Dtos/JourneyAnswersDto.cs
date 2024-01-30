using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class JourneyAnswersDto
    {
        public int JourneyId { get; set; }

        public int? Month { get; set; }

        public double? Tonnes { get; set; }

        public bool? BaledWithWire { get; set; }

        public double? Adjustment { get; set; }

        public string Note { get; set; }

        public string WasteSubType { get; set; }

        public DoneWaste DoneWaste { get; set; }

        public bool? Completed { get; set; }

    }
}
