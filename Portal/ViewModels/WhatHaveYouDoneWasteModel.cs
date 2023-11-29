using EPRN.Common.Enum;
using Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class WhatHaveYouDoneWasteModel
    {
        public int JourneyId { get; set; }

        public string? WasteType { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(WhatHaveYouDoneWithWasteResource))]
        public DoneWaste? WhatHaveYouDone { get; set; }
        
    }
}