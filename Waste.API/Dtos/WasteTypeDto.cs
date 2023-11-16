using Waste.API.Models;

namespace Waste.API.Dtos
{
    public class WasteTypeDto
    {
        public string Name { get; set; } = string.Empty;

        public IEnumerable<WasteSubTypeDto> Subtypes { get; set; }
    }
}
