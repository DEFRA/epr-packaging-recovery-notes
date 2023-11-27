using System.ComponentModel.DataAnnotations;

namespace Waste.API.Models
{
    public class WasteSubType : IdBaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; } = null;

        public double? Adjustment { get; set; } = null;

        public int WasteTypeId { get; set; }

        public virtual WasteType? WasteType { get; set; }

        public virtual ICollection<WasteJourney>? WasteJourneys { get; set; }
    }
}