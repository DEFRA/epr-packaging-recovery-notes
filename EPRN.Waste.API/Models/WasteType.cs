using System.ComponentModel.DataAnnotations;

namespace EPRN.Waste.API.Models
{
    public class WasteType : IdBaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<WasteJourney> Journeys { get; set; }

        public virtual ICollection<WasteSubType> SubTypes { get; set; }
    }
}
