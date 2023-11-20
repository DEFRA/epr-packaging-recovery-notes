using System.ComponentModel.DataAnnotations;
using Waste.API.Models;

namespace Waste.API.Dtos
{
    public class WasteSubTypeDto
    {
        public string Name { get; set; }

        public decimal Adjustment { get; set; }

        public int WasteTypeId { get; set; }

        public WasteTypeDto WasteType { get; set; }
    }
}
