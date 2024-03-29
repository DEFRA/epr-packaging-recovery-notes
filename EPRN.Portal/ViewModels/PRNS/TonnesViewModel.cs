﻿using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class TonnesViewModel
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "TonnesNotInRange")]
        public double? Tonnes { get; set; }
    }
}
