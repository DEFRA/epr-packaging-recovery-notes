using System.ComponentModel.DataAnnotations;
using WasteManagement.API.Models;

namespace Waste.API.Models
{
    public class WasteSubType : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Adjustment { get; set; }

        public int WasteTypeId { get; set; }

        public WasteType WasteType { get; set; }
    }
}