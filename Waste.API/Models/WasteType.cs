using System.ComponentModel.DataAnnotations;
using WasteManagement.API.Models;

namespace Waste.API.Models
{
    public class WasteType : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        public ICollection<WasteSubType> Subtypes { get; set; }

        public ICollection<WasteJourney> WasteJourneys { get; set; }
    }
}
