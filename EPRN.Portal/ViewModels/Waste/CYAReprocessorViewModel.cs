using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.Waste
{
    public class CYAReprocessorViewModel : CYABaseViewModel
    {
        public DoneWaste DoneWaste { get; set; }
        public string ReprocessorWhereWasteSentName { get; internal set; }
        public string ReprocessorWhereWasteSentAddress { get; internal set; }
    }
}
