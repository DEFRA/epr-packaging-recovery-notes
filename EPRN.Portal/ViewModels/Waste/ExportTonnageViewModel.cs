using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class ExportTonnageViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "MissingTonnageError")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(ExportTonnageResources), ErrorMessageResourceName = "TonnesNotInRange")]
        public double? ExportTonnes { get; set; }

        public Category Category { get; set; }
    }
}