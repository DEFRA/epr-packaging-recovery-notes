using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class WasteSubTypesViewModel
    {
        public int Id { get; set; }

        public List<WasteSubTypeOptionViewModel> WasteSubTypeOptions { get; set; }

        [Required(ErrorMessageResourceName = "MissingWasteSubTypeSelectionMessage", ErrorMessageResourceType = typeof(WasteSubTypesResources))]
        public int? SelectedWasteSubTypeId { get; set; }

        public bool AdjustmentRequired
        {
            get
            {
                var selectedOption = WasteSubTypeOptions?.FirstOrDefault(opt => opt.Id == SelectedWasteSubTypeId);

                return selectedOption == null ? false : selectedOption.AdjustmentPercentageRequired;
            }
        }

        [Required(ErrorMessageResourceName = "AdjustmentPercentageRequiredMessage", ErrorMessageResourceType = typeof(WasteSubTypesResources))]
        [Range(0, 100, ErrorMessageResourceName = "InvalidPercentageMessage", ErrorMessageResourceType = typeof(WasteSubTypesResources))]
        public double? CustomPercentage { get; set; }
    }
}
