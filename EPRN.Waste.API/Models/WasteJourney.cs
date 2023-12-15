using EPRN.Common.Enums;

namespace EPRN.Waste.API.Models
{
    public class WasteJourney : BaseEntity
    {
        public double? Quantity { get; set; }

        // some waste sub types have a manual entry for the adjustment, therefore
        // this implies the adjustment from the sub type is either copied over, or
        // the manual entry is used
        public double? Adjustment { get; set; }

        public int? Month { get; set; }

        /// <summary>
        /// Raw tonnage entered by user
        /// </summary>
        public double? Tonnes { get; set; }

        /// <summary>
        /// Total amount including any adjustments
        /// </summary>
        public double? Total { get; set; }

        public bool? BaledWithWire { get; set; }

        public string? Note { get; set; }

        public bool? Completed { get; set; }

        public int? WasteTypeId { get; set; }

        public WasteType WasteType { get; set; }

        public int? WasteSubTypeId { get; set; }

        public WasteSubType WasteSubType { get; set; }

        public string ReferenceNumber { get; set; }

        public DoneWaste? DoneWaste { get; set; }

        public double? DeductionAmount { get; set; }

        public int? SiteId { get; set; }
    }
}