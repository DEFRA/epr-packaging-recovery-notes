using EPRN.Common.Enums;

namespace Portal.ViewModels
{
    public class WasteRecordStatusViewModel
    {
        public int JourneyId { get; set; }
        public WasteRecordStatuses WasteRecordStatus { get; set; }
        public string WasteRecordReferenceNumber { get; set; } = string.Empty;
        public double WasteBalance { get; set; }
    }
}
