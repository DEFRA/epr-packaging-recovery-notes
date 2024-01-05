using System.ComponentModel.DataAnnotations;

namespace EPRN.Common.Data.DataModels
{
    public class WasteType : IdBaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        public ICollection<WasteJourney> Journeys { get; set; }

        public ICollection<WasteSubType> SubTypes { get; set; }
    }
}
