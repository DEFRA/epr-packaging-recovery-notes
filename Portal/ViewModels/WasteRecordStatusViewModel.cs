using EPRN.Common.Enums;

namespace Portal.ViewModels
{
    public class WasteRecordStatusViewModel
    {
        public int ReprocessorId { get; set; }
        public string ReprocessorName { get; set; } = string.Empty;
        public WasteRecordStatuses WasteRecordStatus { get; set; }
        public string WasteRecordStatusMessage { get; set; } = string.Empty;
        public string WasteRecordReferenceNumber { get; set; } = string.Empty;
        public int WasteBalance { get; set; }
    }
}
