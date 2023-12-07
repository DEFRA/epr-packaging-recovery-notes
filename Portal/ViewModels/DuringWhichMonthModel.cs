using EPRN.Common.Enums;
using EPRN.Portal.CustomValidationAttributes;

namespace EPRN.Portal.ViewModels
{
    public class DuringWhichMonthRequestViewModel
    {
        public int JourneyId { get; set; }

        public Dictionary<int, string> Quarter { get; set; } = new Dictionary<int, string>();

        [RequiredWhichMonth(nameof(WhatHaveYouDone))]
        public int? SelectedMonth { get; set; }

        public string WasteType { get; set; }

        public DoneWaste WhatHaveYouDone { get; set; }
    }
}