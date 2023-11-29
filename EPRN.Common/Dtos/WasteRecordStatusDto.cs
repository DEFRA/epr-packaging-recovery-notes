using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class WasteRecordStatusDto
    {
        public int JourneyId { get; set; }
        public WasteRecordStatuses WasteRecordStatus { get; set; }
        public string WasteRecordReferenceNumber { get; set; } = string.Empty;
        public double WasteBalance { get; set; }
    }
}
