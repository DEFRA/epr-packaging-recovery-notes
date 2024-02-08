using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;
namespace EPRN.Portal.ViewModels.PRNS
{
    public class ActionPrnViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(WhatHaveYouDoneWithWasteResource))]
        public PrnStatus? DoWithPRN { get; set; }
    }
}