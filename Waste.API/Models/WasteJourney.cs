using WasteManagement.API.Models;

namespace Waste.API.Models
{
    public class WasteJourney : BaseEntity
    {
        public double? Quantity { get; set; }

        // some waste sub types have a manual entry for the adjustment, therefore
        // this implies the adjustment from the sub type is either copied over, or
        // the manual entry is used
        public double? Adjustment { get; set; }

        public int? Month { get; set; }

        public double? Total { get; set; }

        public bool? BaledWithWire { get; set; }

        public string? Note { get; set; }

        public bool? Completed { get; set; }

        public int? WasteTypeId { get; set; }

        public virtual WasteType? WasteType { get; set; }

        public int? WasteSubTypeId { get; set; }

        public virtual WasteSubType? WasteSubType { get; set; }
    }
}