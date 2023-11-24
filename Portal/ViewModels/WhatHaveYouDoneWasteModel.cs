using Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class WhatHaveYouDoneWasteModel
    {
        public int JourneyId { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(WasteResource))]
        public string? SelectedWaste { get; set; }
    }
}