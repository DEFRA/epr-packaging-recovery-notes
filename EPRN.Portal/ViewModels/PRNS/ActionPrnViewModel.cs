using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;
namespace EPRN.Portal.ViewModels.PRNS
{
    public class ActionPrnViewModel
    {
        public int JourneyId { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(WhatHaveYouDoneWithWasteResource))]
        public ActionPRN? DoWithPRN { get; set; }
    }
}