using System.ComponentModel.DataAnnotations;

namespace Waste.API.Models
{
    public class WasteType : IdBaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<WasteJourney> Jounreys { get; set; }

        public virtual ICollection<WasteSubType> SubTypes { get; set; }
    }
}
