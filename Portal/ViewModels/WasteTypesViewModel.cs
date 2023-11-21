namespace Portal.ViewModels
{
    public class WasteTypesViewModel
    {
        public int JourneyId { get; set; }

        public Dictionary<int, string> WasteTypes { get; set; }

        public int? SelectedWasteType { get; set; }
    }
}
