using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class WhatHaveYouDoneWasteModel
    {
        public int JourneyId { get; set; }

        public string WasteType { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(WhatHaveYouDoneWithWasteResource))]
        public DoneWaste? WhatHaveYouDone { get; set; }
    }
}