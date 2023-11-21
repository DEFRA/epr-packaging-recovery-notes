using Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace PRN.Web.Models
{
    public class WhatHaveYouDoneWasteModel
    {
        public int JourneyId { get; set; }

        public Dictionary<int, string> Quarter { get; set; } = new Dictionary<int, string>();

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(WasteResource))]
        public int? SelectedMonth { get; set; }
    }
}